using System;
using System.Collections.Generic;
using System.Linq;
using _Data;
using UnityEngine;

namespace _Config.So
{
    [CreateAssetMenu(fileName = "MandatoryOccasionSo", menuName = "配置/场合/强制")]
    public class MandatoryOccasionSo : PurposeOccasionBase
    {
        [SerializeField] private ChallengeArgsBase challengeArgs;
        [SerializeField] private TagTermField[] _terms;
        public override bool IsMandatory => true;

        public override ITagTerm[] GetExcludedTerms(IRoleData role) =>
            _terms.GetExcludedTerms(role).ToArray();
        public override IChallengeArgs ChallengeArgs => challengeArgs;//由自己处理，并结合子类的处理

        public override void CheckTags()
        {
            foreach (var field in _terms) field.CheckTag(this);
            challengeArgs.CheckTags();
        }
    }
}