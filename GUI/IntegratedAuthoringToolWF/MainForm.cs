﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Equin.ApplicationFramework;
using GAIPS.AssetEditorTools;
using IntegratedAuthoringTool;
using IntegratedAuthoringTool.DTOs;
using WellFormedNames;
using RolePlayCharacter;

namespace IntegratedAuthoringToolWF
{
	public partial class MainForm : BaseIATForm
	{
        
        private BindingListView<GUIDialogStateAction> _playerDialogs;
        private BindingListView<GUIDialogStateAction> _agentDialogs;
        private readonly string PLAYER = IATConsts.PLAYER;
        private readonly string AGENT = IATConsts.AGENT;
        private BindingListView<CharacterSourceDTO> _characterSources;
		private readonly RolePlayCharacterWF.MainForm _rpcForm = new RolePlayCharacterWF.MainForm();

		private Dictionary<string,Form> _openedForms = new Dictionary<string, Form>();

        private int stateCounter;
        private int totalSize;

        public MainForm()
		{
			InitializeComponent();

			buttonEditCharacter.Enabled = false;
			buttonRemoveCharacter.Enabled = false;
            _playerDialogs = new BindingListView<GUIDialogStateAction>(new List<GUIDialogStateAction>());
            dataGridViewPlayerDialogueActions.DataSource = _playerDialogs;
           // RefreshPlayerDialogs();

            _agentDialogs = new BindingListView<GUIDialogStateAction>(new List<GUIDialogStateAction>());
            dataGridViewAgentDialogueActions.DataSource = _agentDialogs;
            //RefreshAgentDialogs();
            stateCounter = 0;
            totalSize = 0;
        }


        private void RefreshPlayerDialogs()
        {
            _playerDialogs.DataSource = LoadedAsset.GetDialogueActionsBySpeaker(
                PLAYER).Select(d => new GUIDialogStateAction(d)).ToList();
            _playerDialogs.Refresh();
            dataGridViewPlayerDialogueActions.Columns["Id"].Visible = false;

        }

        private void RefreshAgentDialogs()
        {
            _agentDialogs.DataSource = LoadedAsset.GetDialogueActionsBySpeaker(
             AGENT).Select(d => new GUIDialogStateAction(d)).ToList();
            _agentDialogs.Refresh();
            dataGridViewAgentDialogueActions.Columns["Id"].Visible = false;
        }

        private void buttonAddPlayerDialogueAction_Click(object sender, EventArgs e)
        {
            new AddOrEditDialogueActionForm(this, true).ShowDialog();
            RefreshPlayerDialogs();
        }

        protected override void OnAssetDataLoaded(IntegratedAuthoringToolAsset asset)
		{
			textBoxScenarioName.Text = asset.ScenarioName;
			textBoxScenarioDescription.Text = asset.ScenarioDescription;
			_characterSources = new BindingListView<CharacterSourceDTO>(asset.GetAllCharacterSources().ToList());
			dataGridViewCharacters.DataSource = _characterSources;
		}

		private void buttonCreateCharacter_Click(object sender, EventArgs e)
		{
			var asset = _rpcForm.CreateAndSaveEmptyAsset(false);
			if (asset == null)
				return;
					
            LoadedAsset.AddNewCharacterSource(new CharacterSourceDTO() {Source = asset.AssetFilePath});
			_characterSources.DataSource = LoadedAsset.GetAllCharacterSources().ToList();
			_characterSources.Refresh();
			SetModified();
		}

		private void buttonAddCharacter_Click(object sender, EventArgs e)
		{
			var rpc = _rpcForm.SelectAndOpenAssetFromBrowser();
			if (rpc == null)
				return;

			LoadedAsset.AddNewCharacterSource(new CharacterSourceDTO()
			{
				Source =  rpc.AssetFilePath
			});

			_characterSources.DataSource = LoadedAsset.GetAllCharacterSources().ToList();
			_characterSources.Refresh();
			SetModified();
		}

		private void textBoxScenarioName_TextChanged(object sender, EventArgs e)
		{
			if (IsLoading)
				return;

			LoadedAsset.ScenarioName = textBoxScenarioName.Text;
			SetModified();
		}

		private void textBoxScenarioDescription_TextChanged(object sender, EventArgs e)
		{
			if (IsLoading)
				return;

			LoadedAsset.ScenarioDescription = textBoxScenarioDescription.Text;
			SetModified();
		}

		private void buttonRemoveCharacter_Click(object sender, EventArgs e)
		{
			IList<int> charactersToRemove = new List<int>();
			for (var i = 0; i < dataGridViewCharacters.SelectedRows.Count; i++)
			{
				var character = ((ObjectView<CharacterSourceDTO>) dataGridViewCharacters.SelectedRows[i].DataBoundItem).Object;
				Form f;
				if (_openedForms.TryGetValue(character.Source,out f))
				{
					var r = MessageBox.Show($"\"{character.Source}\" is currently being edited.\nDo you wish to remove it?", "Warning",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (r == DialogResult.No)
						continue;

					f.Close();
				}
				charactersToRemove.Add(character.Id);
			}

			LoadedAsset.RemoveCharacters(charactersToRemove);
			_characterSources.DataSource = LoadedAsset.GetAllCharacterSources().ToList();
			_characterSources.Refresh();
			SetModified();
		}

		private void buttonEditCharacter_Click(object sender, EventArgs e)
		{
			for (var i = 0; i < dataGridViewCharacters.SelectedRows.Count; i++)
			{
				var character = ((ObjectView<CharacterSourceDTO>) dataGridViewCharacters.SelectedRows[i].DataBoundItem).Object;
				if(_openedForms.ContainsKey(character.Source))
					continue;

				var form = new RolePlayCharacterWF.MainForm();
				form.Closed += (o, args) =>
				{
					_openedForms.Remove(character.Source);
					ReloadEditor();
				};
                var rpc = RolePlayCharacterAsset.LoadFromFile(character.Source);
				form.EditAssetInstance(() => rpc);
				_openedForms.Add(character.Source,form);
				form.Show();
			}
		}

		#region Toolbar Options

		private bool _dialogEditorIsShowing;

		[MenuItem("Tools/Show Dialog Editor #ctrl+D")]
		private void ShowToolbar()
		{
			var d = new DialogueEditorForm(this);
			_dialogEditorIsShowing = true;
			d.Closed += (sender, args) => { _dialogEditorIsShowing = false; };
			d.Show();
		}

		[MenuItem("Tools/Show Dialog Editor")]
		private bool ShowToolbar_Validation()
		{
			return !_dialogEditorIsShowing;
		}
		#endregion

		#region About

		[MenuItem("About",Priority = int.MaxValue)]
		private void ShowAbout()
		{
			var form = new AboutForm();
			form.ShowDialog();
		}

		#endregion

		private void dataGridViewCharacters_SelectionChanged(object sender, EventArgs e)
		{
			var active = dataGridViewCharacters.SelectedRows.Count > 0;
			buttonEditCharacter.Enabled = active;
			buttonRemoveCharacter.Enabled = active;
		}

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void groupBoxDialogueEditor_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}