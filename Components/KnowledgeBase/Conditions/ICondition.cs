﻿using System.Collections.Generic;
using System.Linq;
using KnowledgeBase.WellFormedNames;

namespace KnowledgeBase.Conditions
{
	public interface ICondition
	{
		IEnumerable<SubstitutionSet> UnifyEvaluate(KB kb, SubstitutionSet constraints);

		bool Evaluate(KB kb, SubstitutionSet constraints);
	}
}