function ToggleNPCEditorGui(%val)
{
	if(%val)
	{
		if(NPCEditorGui.isAwake())
			NPCEditorGui_Done();
		else
			Canvas.pushDialog("NPCEditorGui");
	}
}

function initNPCEditorGui()
{
	if(!isObject(NPCEditorGui))
	{
		//We want our cmd to end up in the "GUI" section
		for(%i=$RemapCount-1;%i>=0;%i--)//First, find the insert location.
		{
			if($RemapDivision[%i]$="Tools / Inventory")
			{
				%insert = %i;
				break;
			}
		}

		if(%insert)
		{
			$RemapCount++;
			for(%i=$RemapCount-1;%i>=0;%i--)//Then shift everything beyond that up by 1, and then insert and quit.
			{
				if(%i==%insert)
				{
					$RemapDivision[%i] = "";
					$RemapName[%i] = "Toggle NPC Editor";
					$RemapCmd[%i] = "ToggleNPCEditorGui";
					OptRemapList.setRowById(%i, buildFullMapString(%i));
					break;
				}
				else
				{
					$RemapDivision[%i] = $RemapDivision[%i-1];
					$RemapName[%i] = $RemapName[%i-1];
					$RemapCmd[%i] = $RemapCmd[%i-1];
				}
			}

			//set up a default bind if none set
			if(moveMap.getBinding($RemapCmd[%inset])$="")
			{
				%prevMap = moveMap.getCommand("keyboard","ctrl -");
				if(%prevmap $= "")
				{
					moveMap.bind("keyboard", "ctrl -", "ToggleNPCEditorGui");
				}
			}
		}
	}

	//Load default preferences.
	if($pref::NPCAvatar::Accent $= "")
	{
	   $pref::NPCAvatar::Accent = "0";
	   $pref::NPCAvatar::AccentColor = "0.000 0.200 0.640 0.700";
	   $Pref::NPCAvatar::Chest = "1";
	   $pref::NPCAvatar::ChestColor = "7";
	   $pref::NPCAvatar::DecalColor = "0";
	   $Pref::NPCAvatar::DecalName = "base/data/shapes/player/decals/AAA-None.png";
	   $pref::NPCAvatar::FaceColor = "0";
	   $Pref::NPCAvatar::FaceName = "smiley";
	   $pref::NPCAvatar::Hat = "0";
	   $pref::NPCAvatar::HatColor = "1 1 0 1";
	   $Pref::NPCAvatar::HatList = "testHat";
	   $Pref::NPCAvatar::Head = "1";
	   $pref::NPCAvatar::HeadColor = "1 0.878431 0.611765 1";
	   $Pref::NPCAvatar::Hip = "1";
	   $pref::NPCAvatar::HipColor = "0 0 1 1";
	   $Pref::NPCAvatar::LArm = "1";
	   $pref::NPCAvatar::LArmColor = "0.900 0.900 0.000 1.000";
	   $Pref::NPCAvatar::LHand = "1";
	   $pref::NPCAvatar::LHandColor = "1 0.878431 0.611765 1";
	   $Pref::NPCAvatar::LLeg = "1";
	   $Pref::NPCAvatar::LLegColor = "0 0 1 1";
	   $Pref::NPCAvatar::LSki = "0";
	   $Pref::NPCAvatar::LSkiColor = "0 0 1 1";
	   $pref::NPCAvatar::Pack = "0";
	   $pref::NPCAvatar::PackColor = "0 0.435323 0.831776 1";
	   $Pref::NPCAvatar::RArm = "1";
	   $pref::NPCAvatar::RArmColor = "0.900 0.900 0.000 1.000";
	   $Pref::NPCAvatar::RHand = "1";
	   $pref::NPCAvatar::RHandColor = "1 0.878431 0.611765 1";
	   $Pref::NPCAvatar::RLeg = "1";
	   $pref::NPCAvatar::RLegColor = "0 0 1 1";
	   $Pref::NPCAvatar::RSki = "0";
	   $Pref::NPCAvatar::RSkiColor = "0 0 1 1";
	   $Pref::NPCAvatar::SecondPack = "0";
	   $pref::NPCAvatar::SecondPackColor = "0 1 0 1";
	   $pref::NPCAvatar::Symmetry = "";
	   $pref::NPCAvatar::TorsoColor = "0.900 0.900 0.000 1.000";
	}

	//Execute functionality
	exec("./NPCEditorGui.cs");

	//Create visuals
	exec("./NPCColorSetGui.gui");
	exec("./NPCEditorGui.gui");
}
initNPCEditorGui();