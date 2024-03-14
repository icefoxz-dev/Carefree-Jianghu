using System;
using System.Linq;
using _Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "OccasionSo", menuName = "配置/场合/一般")]
    public class FuncOccasionSo : PurposeOccasionBase,IChallengeArgs
    {
        [SerializeField] private SceneContent _sceneContent;
        [SerializeField] private TagTermField[] terms;
        [SerializeField,FormerlySerializedAs("results")] private RewardTag[] rewards;
        public override ITagValue[] GetRewards(IOccasionResult result) => rewards;

        public override IChallengeArgs ChallengeArgs => this;
        public ChallengeTypes ChallengeType => ChallengeTypes.None;

        public SceneContent SceneContent => _sceneContent;
        public override bool IsMandatory => false;

        public override ITagTerm[] GetExcludedTerms(IRoleData role)
        {
            foreach (var tag in terms.Select(t=>t.RoleTag))
                if (!tag)
                    Debug.LogError("game tag not set!", this);
            return terms.GetExcludedTerms(role);
        }
    }
}