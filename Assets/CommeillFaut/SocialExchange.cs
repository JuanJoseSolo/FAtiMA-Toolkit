﻿using System;
using System.Collections.Generic;
using ActionLibrary;
using ActionLibrary.DTOs;
using CommeillFaut.DTOs;
using Conditions;
using Conditions.DTOs;
using EmotionalAppraisal;
using KnowledgeBase;
using WellFormedNames;
using SerializationUtilities;

namespace CommeillFaut
{

    [Serializable]
    public class SocialExchange : BaseActionDefinition
    {
    


        public String Intent { get; set; }

     

        public string Initiator { get; set; }

       public string targ { get; set; }

        int Response { get; set; }

        public String State { get; set; }

        public String Instantiation { get; set; }

        public List<InfluenceRuleDTO> InfluenceRules { get; set; }

        public Dictionary<int,List<string>> EffectsList { get; set; }

        public SocialExchange(String name) : base(WellFormedNames.Name.BuildName(name), WellFormedNames.Name.BuildName("Empty"), new ConditionSet(new ConditionSetDTO()))
        
            {
            State = "Initialized";
     
            Intent = "";
            Instantiation = "";

            InfluenceRules = new List<InfluenceRuleDTO>();
            EffectsList = new Dictionary<int,List<string>>();

        }


        public SocialExchange(SocialExchangeDTO s) : base(s)
        {

         

            InfluenceRules = new List<InfluenceRuleDTO>();
            //      Name = s.SocialExchangeName;
            if(s.InfluenceRules!=null)
            foreach (var inf in s.InfluenceRules)
            {
                InfluenceRules.Add(inf);
            }
            EffectsList = s.Effects ?? new Dictionary<int, List<string>>();
            Intent = s.Intent;
            Instantiation = s.Instantiation;
        }

        public SocialExchange(SocialExchange other) : base(other)
		{
         
            Intent = other.Intent;
		    Instantiation = other.Instantiation;

            if (other.InfluenceRules != null)
                foreach (var inf in other.InfluenceRules)
                {
                    InfluenceRules.Add(inf);
                }

		    EffectsList = other.EffectsList;
		}



        private void Instatiate(int response)
        {
            var write = "Instantiating SocialExchange " + ActionName + " \n";


            write += Initiator + " wants to " + Intent + " with " + targ + "\n";

            Console.WriteLine(write);


           if(response > 0)
                write = targ + " accepted the " + this.ActionName + " Social Exchange \n";
           else if (response == 0)
                write = targ + " is neutral to the " + this.ActionName + " Social Exchange \n";
           else 
                write = targ + " rejected the " + this.ActionName + " Social Exchange \n";


            Console.WriteLine(write);


          
            write = " Social Exchange" + base.ActionName + " completed \n";
            Console.WriteLine(write);
            //System.Threading.Thread.Sleep(2000);
        }

        public int CalculateVolition(string init, string _targ, KB m_Kb)
        {

            int counter = 0;
            foreach (var rule in InfluenceRules)
            {
             
                var result = new InfluenceRule(rule);
                counter += result.Result(init, _targ, m_Kb);
                Console.WriteLine("SocialExchange : " + this.ActionName.ToString() + "Initiator " + init + "Target " + _targ+ " Rule: " + rule.RuleName + " Counter" + counter);

            }
            return counter;
        }


        private int CalculateResponse(string Init, string _Targ, KB m_Kb)
        {
           // var write = "Calculating SocialExchangeResponse:";// Target.CalculateResponse(Name, Initiator) + "\n";

            return this.CalculateVolition(_Targ, Init, m_Kb);

           
        }

        public override String ToString()
        {

            return base.ActionName + " " + Intent + " " + "\n";
        }


        public void LaunchSocialExchange(string init, string _targ, KB init_Kb, KB targ_Kb)
        {
            Initiator = init;
            targ = _targ;

            var write = "Launching SocialExchange: " + base.ActionName + " Initator: " + init + " Target: " + targ + "\n";

            Console.WriteLine(write);

           
            var response = CalculateResponse(init, targ, targ_Kb);
            Console.WriteLine("Response result: " + response );

            Instatiate(response);

            ApplyConsequences(init_Kb, targ_Kb, response);

            State = "Completed";


            write = " SocialExchange Completed: " + base.ActionName + " Initator: " + init + " Target: " + targ + " result : " + response + "\n";

            Console.WriteLine(write);

        }



        public void ApplyConsequences(KB init_Kb , KB targ_Kb, int response)
        {


            if (EffectsList.ContainsKey(response))
            {
                foreach (var effect in EffectsList[response])
                {
                    ApplyKeywordEffects(init_Kb, targ_Kb, effect);
                }

            }

           

        }

        public void ApplyKeywordEffects(KB init_Kb, KB targ_Kb, string keyword)
        {
            char[] delimitedChars = {'(', ')', ','};

            string[] words = keyword.Split(delimitedChars);
            var value = 0;

                                             // Ideally we would be able to insert any 
                                          // social network but we don't store them just yet
                if (words[1] == "Initiator")
                {
                    if(words[2] == "Target")
                    {

                        if (init_Kb.AskProperty((Name) (words[0] + "(" + targ_Kb.Perspective + ")")) != null)
                        {
                             value =
                                Convert.ToInt32(init_Kb.AskProperty((Name) (words[0] + "(" + targ_Kb.Perspective + ")")).ToString());
                            value += Convert.ToInt32(words[3]);

                        }
                        else
                        {
                             value = Convert.ToInt32(words[3]);
                        }

                        var insert = "" + value;
                        init_Kb.Tell((Name)(words[0] + "(" + targ_Kb.Perspective + ")"), (Name)insert);
                        return;
                    }
                }

            if (words[1] == "Target")
            {
                if (words[2] == "Initiator")
                {
                    if (init_Kb.AskProperty((Name) (words[0] + "(" + init_Kb.Perspective + ")")) != null)
                    {
                         value =
                            Convert.ToInt32(init_Kb.AskProperty((Name) (words[0] + "(" + init_Kb.Perspective + ")")).ToString());
                        value += Convert.ToInt32(words[3]);
                        string insert = "" + value;
                        targ_Kb.Tell((Name) (words[0] + "(" + init_Kb.Perspective + ")"), (Name) insert);
                        return;
                    }
                    else
                    {

                         value = Convert.ToInt32(words[3]);
                        string insert = "" + value;
                        targ_Kb.Tell((Name)(words[0] + "(" + init_Kb.Perspective + ")"), (Name)insert);
                        return;
                    } 
                }
            }




        }

        public void SetData(SocialExchangeDTO dto)
        {
            
            Intent = dto.Intent;
            Instantiation = dto.Instantiation;

            foreach (var go in dto.InfluenceRules)
            {
                InfluenceRules.Add(go);
            }
        
            SetFromDTO(dto);
        }

        public SocialExchangeDTO ToDTO()
        {
            List<InfluenceRuleDTO> ret = new List<InfluenceRuleDTO>();

            foreach (var inf in InfluenceRules)
            {
               ret.Add(inf);
            }
            return new SocialExchangeDTO() {Action = ActionName.ToString(), Intent = Intent, Instantiation = Instantiation, InfluenceRules = ret, Effects = EffectsList};
        }


       public override void GetObjectData(ISerializationData dataHolder, ISerializationContext context)
        {

         
            dataHolder.SetValue("Intent", this.Intent);
            dataHolder.SetValue("Instantiation", this.Instantiation);
            dataHolder.SetValue("InfluenceRules", this.InfluenceRules);
            dataHolder.SetValue("EffectsList", this.EffectsList);


            foreach (var inf in this.InfluenceRules)
            {

                dataHolder.SetValue(inf.RuleName, inf.RuleConditions);

            }
            
            base.GetObjectData(dataHolder, context);
           
           
        }

        public override void SetObjectData(ISerializationData dataHolder, ISerializationContext context)
        {
            base.SetObjectData(dataHolder, context);
           
            Intent = dataHolder.GetValue<string>("Intent");
            Instantiation = dataHolder.GetValue<string>("Instantiation");
            InfluenceRules = dataHolder.GetValue<List<InfluenceRuleDTO>>("InfluenceRules");
            EffectsList = dataHolder.GetValue<Dictionary<int,List<string>>>("EffectsList");
         
            foreach (var inf in InfluenceRules)
            {

              var InfConditions = dataHolder.GetValue<ConditionSetDTO>(inf.RuleName);
               inf.RuleConditions =InfConditions;
            }
        }

        public void AddInfluenceRule(InfluenceRule infrul)
        {

            InfluenceRules.Add(infrul.ToDTO());

        }

        public void EditInfluenceRule(InfluenceRule infrul)
        {
            InfluenceRules.Remove(InfluenceRules.Find(x => x.RuleName == infrul.RuleName));
            InfluenceRules.Add(infrul.ToDTO());

        }


        public void RemoveInfluenceRule(InfluenceRule infrul)
        {
            InfluenceRules.Remove(InfluenceRules.Find(x => x.RuleName == infrul.RuleName));
          

        }

        protected override float CalculateActionUtility(IAction a)
        {
            throw new NotImplementedException();
        }


    }
}