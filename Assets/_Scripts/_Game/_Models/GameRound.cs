using System;
using _Data;
using System.Linq;
using UnityEngine.Events;

namespace _Game._Models
{
    /// <summary>
    /// 游戏回合，处理回合，游戏时间等逻辑
    /// </summary>
    public class GameRound : ModelBase , IGameRound
    {
        public int Year => GameDate.Year;
        public int Month => GameDate.Month;
        public int Round => GameDate.Round;
        public IStoryMap StoryMap { get; }
        public IGameDate GameDate => _gameDate;

        public IPurpose[] Purposes { get; private set; }
        //public IChallenge Challenge { get; private set; }
        public IPurpose SelectedPurpose { get; private set; }
        public Challenge.SimpleBattle Battle { get; private set; }
        public ChallengeTypes ChallengeType { get; private set; }
        public IOccasion Occasion => _currentOccasion;
        public bool IsMandatory { get; private set; }

        private UnityAction<IOccasionResult> _challengeCallback;
        private IOccasion _currentOccasion;
        private readonly GameDate _gameDate;

        public GameRound(IStoryMap storyMap, GameDate gameDate)
        {
            StoryMap = storyMap;
            _gameDate = gameDate;
        }

        public void NextRound()
        {
            _gameDate.NextRound();
            SelectedPurpose = null;
        }

        /// <summary>
        /// 更新可执行的意图
        /// </summary>
        /// <param name="role"></param>
        public void UpdatePurposes(IRoleData role)
        {
            var mandatory = StoryMap.Activities.GetMandatoryPurposes(role, this);
            var clusters = StoryMap.Activities.GetClusters();
            var purposes = clusters.SelectMany(c => c.GetPurposes(role, this)).ToArray();
            mandatory = mandatory.Concat(purposes.Where(p => p.IsMandatory)).ToArray();
            IsMandatory = mandatory.Length > 0;
            Purposes = IsMandatory
                ? mandatory
                : purposes;
            SendEvent(GameEvent.Round_Puepose_Update);
        }
        /// <summary>
        /// 选择意图
        /// </summary>
        public void SelectPurpose(IRoleData role, IPurpose purpose)
        {
            SelectedPurpose = purpose;
            _currentOccasion = SelectedPurpose.GetOccasion(role);
            SendEvent(GameEvent.Round_Puepose_Update);
        }


        public void InstanceChallenge(IRoleData role, UnityAction<IOccasionResult> callback)
        {
            _challengeCallback = callback;
            SendEvent(GameEvent.Occasion_Update);
            ChallengeType = _currentOccasion.ChallengeArgs.ChallengeType;
            switch (_currentOccasion.ChallengeArgs.ChallengeType)
            {
                case ChallengeTypes.Battle:
                    var arg = (IChallengeBattleArgs)_currentOccasion.ChallengeArgs;
                    var opponent = arg.GetOpponent(Game.Config.CharacterTagsMap.GetCapableTags);
                    Battle = new Challenge.SimpleBattle(role, opponent, _challengeCallback);
                    break;
                case ChallengeTypes.MiniGame:
                    throw new NotImplementedException("暂时没有小游戏!");
                case ChallengeTypes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void ChallengeStart()
        {
            switch (_currentOccasion.ChallengeArgs.ChallengeType)
            {
                case ChallengeTypes.Battle:
                    Battle.Start();
                    SendEvent(GameEvent.Challenge_Battle_Start);
                    break;
                case ChallengeTypes.MiniGame:
                    throw new NotImplementedException("暂时没有小游戏!");
                    SendEvent(GameEvent.Challenge_Mini_Start);
                case ChallengeTypes.None:
                    _challengeCallback(Challenge.DefaultResult);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ChallengeFinalize()
        {
            switch (_currentOccasion.ChallengeArgs.ChallengeType)
            {
                case ChallengeTypes.Battle:
                    Battle.Finalize();
                    break;
                case ChallengeTypes.MiniGame:
                    throw new NotImplementedException("暂时没有小游戏!");
                case ChallengeTypes.None:
                    throw new InvalidOperationException("Challenge.Type.None 应该直接回调。");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ITagTerm[] GetExcludedTerms(IRoleData role) => _currentOccasion.GetExcludedTerms(role);
    }
}