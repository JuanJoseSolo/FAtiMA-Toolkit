﻿using System.Windows.Forms;
using RolePlayCharacter;
using Equin.ApplicationFramework;
using ActionLibrary;
using System.Linq;
using GAIPS.AssetEditorTools;
using IntegratedAuthoringTool;
using System.Collections;
using System;
using System.Collections.Generic;
using WellFormedNames;
using AutobiographicMemory;
using EmotionalAppraisal.DTOs;

namespace IntegratedAuthoringToolWF
{
    public partial class RPCInspectForm : Form
    {
        private string rpcSource;
        private IntegratedAuthoringToolAsset iat;

        private BindingListView<IAction> actions;
        private BindingListView<EmotionDTO> emotions;

        public RPCInspectForm(IntegratedAuthoringToolAsset iatAsset, string rpcSource)
        {
            InitializeComponent();
            this.iat = iatAsset;
            this.rpcSource = rpcSource;

            EditorTools.AllowOnlyGroundedLiteralOrUniversal(wfNameActionLayer);
            wfNameActionLayer.Value = WellFormedNames.Name.UNIVERSAL_SYMBOL;

            this.Text += " - " + rpcSource;

            actions = new BindingListView<IAction>((IList)null);
            dataGridViewDecisions.DataSource = actions;
            EditorTools.HideColumns(dataGridViewDecisions, new[]
            {
                PropertyUtil.GetPropertyName<IAction>(a => a.Parameters)
            });

            emotions = new BindingListView<EmotionDTO>((IList)null);
            dataGridActiveEmotions.DataSource = emotions;
        }

        private void buttonTest_Click(object sender, System.EventArgs e)
        {
            var rpcAsset = RolePlayCharacterAsset.LoadFromFile(rpcSource);
            rpcAsset.LoadAssociatedAssets();
            iat.BindToRegistry(rpcAsset.DynamicPropertiesRegistry);

            string[] eventStrings = this.textBoxEvents.Text.Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.None).Select(s => s.Trim()).ToArray();

            foreach(var s in eventStrings)
            {
                if (string.IsNullOrWhiteSpace(s))
                    continue;
                    
                if (s.StartsWith("-"))
                {
                    var count = s.Count(c => c == '-');
                    for (int i = 0; i < count; i++)
                        rpcAsset.Update();
                }
                else
                {
                    try
                    {
                        var evt = WellFormedNames.Name.BuildName(s);
                        AM.AssertEventNameValidity(evt);
                        rpcAsset.Perceive(evt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            textBoxMood.Text = rpcAsset.Mood.ToString();
            textBoxTick.Text = rpcAsset.Tick.ToString();
            emotions.DataSource = rpcAsset.GetAllActiveEmotions().ToList();
            actions.DataSource = rpcAsset.Decide(wfNameActionLayer.Value).ToList();
        }

        private void RPCInspectForm_Load(object sender, EventArgs e)
        {

        }
    }
}
