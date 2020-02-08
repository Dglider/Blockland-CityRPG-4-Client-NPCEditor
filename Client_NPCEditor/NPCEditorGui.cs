///////////////////////
//NPC Editor Gui Code//
///////////////////////

//Some of this code was pulled from a badly decompiled version of the Avatar Editor Gui
//I fixed it up and renamed a ton of stuff.  It should work just fine but... well.
//It's certainly not how I would have coded it.

///////////////////////////////////
//Standard NPCEditorGui Functions//
///////////////////////////////////

function NPCEditorGui::onWake(%this)//Initialize all the menus
{
   //NPCMessage_Section
   NPCMessage_Display.attach(NPCMessage_Vector);//The message vector is automatically detached every time the GUI closes for some reason.
   %this.addMessage("\c1Editor opened...");

	//NPCBrick_Section
	if(isObject(ServerConnection))
	{
		NPCBrick_List.requestNPCSpawnList();//Request the spawn list
		commandToServer('requestNPCSpawnSelect');//Then request the brick in front of our face.  (This function will wait for the list request to be fullfilled)
	}

   //NPCAvatar_Section
   //Don't ask... most of this entire section was not written by me.
   //I'll comment on what I changed.
   if ($NPCAvatar::NumColors $= "")
   {
      NPCColorSetGui.load();
   }
   NPCEditorGui.updateFavButtons();
   NPCED_FavsHelper.setVisible(0);
   NPCAvatar_SymmetryCheckbox.setValue(mAbs($pref::NPCAvatar::Symmetry));

   //On wake, store avatar prefs in case we want do undo something. >.<
   //We removed the Avatar::Authentic field from everything.
   //It seems to have been a throwback to updates from older versions of Blockland
   //But in the current version it did literally nothing.

   $Old::NPCAvatar::Accent = $Pref::NPCAvatar::Accent;
   $Old::NPCAvatar::AccentColor = $Pref::NPCAvatar::AccentColor;
   $Old::NPCAvatar::Chest = $Pref::NPCAvatar::Chest;
   $Old::NPCAvatar::ChestColor = $Pref::NPCAvatar::ChestColor;
   $Old::NPCAvatar::DecalColor = $Pref::NPCAvatar::DecalColor;
   $Old::NPCAvatar::DecalName = $Pref::NPCAvatar::DecalName;
   $Old::NPCAvatar::FaceColor = $Pref::NPCAvatar::FaceColor;
   $Old::NPCAvatar::FaceName = $Pref::NPCAvatar::FaceName;
   $Old::NPCAvatar::Hat = $Pref::NPCAvatar::Hat;
   $Old::NPCAvatar::HatColor = $Pref::NPCAvatar::HatColor;
   $Old::NPCAvatar::Head = $Pref::NPCAvatar::Head;
   $Old::NPCAvatar::HeadColor = $Pref::NPCAvatar::HeadColor;
   $Old::NPCAvatar::Hip = $Pref::NPCAvatar::Hip;
   $Old::NPCAvatar::HipColor = $Pref::NPCAvatar::HipColor;
   $Old::NPCAvatar::LArm = $Pref::NPCAvatar::LArm;
   $Old::NPCAvatar::LArmColor = $Pref::NPCAvatar::LArmColor;
   $Old::NPCAvatar::LHand = $Pref::NPCAvatar::LHand;
   $Old::NPCAvatar::LHandColor = $Pref::NPCAvatar::LHandColor;
   $Old::NPCAvatar::LLeg = $Pref::NPCAvatar::LLeg;
   $Old::NPCAvatar::LLegColor = $Pref::NPCAvatar::LLegColor;
   $Old::NPCAvatar::LSki = $Pref::NPCAvatar::LSki;
   $Old::NPCAvatar::LSkiColor = $Pref::NPCAvatar::LSkiColor;
   $Old::NPCAvatar::Pack = $Pref::NPCAvatar::Pack;
   $Old::NPCAvatar::PackColor = $Pref::NPCAvatar::PackColor;
   $Old::NPCAvatar::RArm = $Pref::NPCAvatar::RArm;
   $Old::NPCAvatar::RArmColor = $Pref::NPCAvatar::RArmColor;
   $Old::NPCAvatar::RHand = $Pref::NPCAvatar::RHand;
   $Old::NPCAvatar::RHandColor = $Pref::NPCAvatar::RHandColor;
   $Old::NPCAvatar::RLeg = $Pref::NPCAvatar::RLeg;
   $Old::NPCAvatar::RLegColor = $Pref::NPCAvatar::RLegColor;
   $Old::NPCAvatar::RSki = $Pref::NPCAvatar::RSki;
   $Old::NPCAvatar::RSkiColor = $Pref::NPCAvatar::RSkiColor;
   $Old::NPCAvatar::SecondPack = $Pref::NPCAvatar::SecondPack;
   $Old::NPCAvatar::SecondPackColor = $Pref::NPCAvatar::SecondPackColor;
   $Old::NPCAvatar::Symmetry = $Pref::NPCAvatar::Symmetry;
   $Old::NPCAvatar::TorsoColor = $Pref::NPCAvatar::TorsoColor;
   if (NPCEditorGui.NPCAvatarHasLoaded)
   {
      NPCAvatar_Preview.setCamera();
      NPCAvatar_Preview.setCameraRot(0.3, 0.6, 2.52);
      NPCAvatar_Preview.setOrbitDist(4.34);
      NPCAvatar_UpdatePreview();
   }
   else
   {
      NPCEditorGui.NPCAvatarHasLoaded = 1;
      NPCEditorGui_LoadAccentInfo("accent", "Add-Ons/Client_NPCEditor/shapes/accent.txt");
      %x = getWord(NPCAvatar_FacePreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_FacePreview.position, 1);
      NPCEditorGui_CreatePartMenuFACE("NPCAvatar_FaceMenu", "NPCAvatar_SetFace", "Add-Ons/Client_NPCEditor/shapes/face.ifl", %x, %y);
      %x = getWord(NPCAvatar_DecalPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_DecalPreview.position, 1) - 64.0;
      NPCEditorGui_CreatePartMenuFACE("NPCAvatar_DecalMenu", "NPCAvatar_SetDecal", "Add-Ons/Client_NPCEditor/shapes/decal.ifl", %x, %y);
      %x = getWord(NPCAvatar_PackPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_PackPreview.position, 1) - 64.0;
      NPCEditorGui_CreatePartMenu("NPCAvatar_PackMenu", "NPCAvatar_SetPack", "Add-Ons/Client_NPCEditor/shapes/Pack.txt", %x, %y);
      %x = getWord(NPCAvatar_SecondPackPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_SecondPackPreview.position, 1) - 64.0;
      NPCEditorGui_CreatePartMenu("NPCAvatar_SecondPackMenu", "NPCAvatar_SetSecondPack", "Add-Ons/Client_NPCEditor/shapes/SecondPack.txt", %x, %y);
      %x = getWord(NPCAvatar_HatPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_HatPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_HatMenu", "NPCAvatar_SetHat", "Add-Ons/Client_NPCEditor/shapes/Hat.txt", %x, %y);
      %x = getWord(NPCAvatar_AccentPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_AccentPreview.position, 1);
      NPCEditorGui_CreateSubPartMenu("NPCAvatar_AccentMenu", "NPCAvatar_SetAccent", $NPCaccentsAllowed[$NPChat[$Pref::NPCAvatar::Hat]], %x, %y);
      %x = getWord(NPCAvatar_ChestPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_ChestPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_ChestMenu", "NPCAvatar_SetChest", "Add-Ons/Client_NPCEditor/shapes/Chest.txt", %x, %y);
      %x = getWord(NPCAvatar_HipPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_HipPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_HipMenu", "NPCAvatar_SetHip", "Add-Ons/Client_NPCEditor/shapes/hip.txt", %x, %y);
      %x = getWord(NPCAvatar_RLegPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_RLegPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_RLegMenu", "NPCAvatar_SetRLeg", "Add-Ons/Client_NPCEditor/shapes/RLeg.txt", %x, %y);
      %x = getWord(NPCAvatar_LLegPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_LLegPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_LLegMenu", "NPCAvatar_SetLLeg", "Add-Ons/Client_NPCEditor/shapes/LLeg.txt", %x, %y);
      %x = getWord(NPCAvatar_RArmPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_RArmPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_RArmMenu", "NPCAvatar_SetRArm", "Add-Ons/Client_NPCEditor/shapes/RArm.txt", %x, %y);
      %x = getWord(NPCAvatar_LArmPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_LArmPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_LArmMenu", "NPCAvatar_SetLArm", "Add-Ons/Client_NPCEditor/shapes/LArm.txt", %x, %y);
      %x = getWord(NPCAvatar_RHandPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_RHandPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_RHandMenu", "NPCAvatar_SetRHand", "Add-Ons/Client_NPCEditor/shapes/RHand.txt", %x, %y);
      %x = getWord(NPCAvatar_LHandPreview.position, 0) + 64.0;
      %y = getWord(NPCAvatar_LHandPreview.position, 1);
      NPCEditorGui_CreatePartMenu("NPCAvatar_LHandMenu", "NPCAvatar_SetLHand", "Add-Ons/Client_NPCEditor/shapes/LHand.txt", %x, %y);
      NPCAvatar_Preview.setObject("", "Add-Ons/Client_NPCEditor/shapes/NPC.dts", "", 100);//We changed over to our own .dts shape here.
      NPCAvatar_Preview.setSequence("", 0, headup, 0);
      NPCAvatar_Preview.setThreadPos("", 0, 0);
      NPCAvatar_Preview.setCameraRot(0.3, 0.6, 2.52);
      NPCAvatar_Preview.setOrbitDist(4.34);
      NPCAvatar_Preview.setScale(0.9, 0.9, 0.9);
      %i = 0;
      while(%i < $numNPCDecal)
      {
         if (fileBase($NPCdecal[%i]) $= $Pref::NPCAvatar::DecalName)
            $Pref::NPCAvatar::DecalColor = %i;
         %i++;
      }
      %i = 0;
      while(%i < $numNPCFace)
      {
         if (fileBase($NPCface[%i]) $= $Pref::NPCAvatar::FaceName)
            $Pref::NPCAvatar::FaceColor = %i;
         %i++;
      }
      NPCAvatar_UpdatePreview();
   }

   //NPCData_Section
   if(!NPCData_Section.hasLoaded)
   {
      //Set up our animation list for use in the poses subsection.
   	%fo = new FileObject();
   	%fo.openForRead("Add-Ons/Client_NPCEditor/shapes/animations.txt");
   	NPCData_Section.numAnimation = 0;
   	while(!%fo.isEOF())
   	{
   		NPCData_Section.animation[NPCData_Section.numAnimation] = %fo.readLine();
   		NPCData_Section.numAnimation++;
   	}
   	%fo.close();
   	%fo.delete();
   	NPCData_Section.hasLoaded = 1;
   }

   //We need to build the event list so that we can apply our changes to the spawn brick.
   if(isObject(ServerConnection))
   {
      if(!ServerConnection.isLocal() && !$GotInputEvents)//$GotInputEvents is cleared when you leave the server, so we can be pretty dang sure that we won't have issues with a badly built event list.
      {
         %this.builtEventList = false;
         deleteVariables("$InputEvent_*");
         deleteVariables("$OutputEvent_*");
         commandToServer('RequestEventTables');
      }
   }
   else
   {
      %this.builtEventList = false;
   }

   //Goddamn FrameSet keeps autosizing it's children. >:(
   for(%i=0;%i<NPCData_Pose_Frame.numPoses;%i++)
   {
      NPCData_Pose_Frame.Pos[%i].extent = "60 18";
      NPCData_Pose_Frame.Rot[%i].extent = "60 18";
   }
}
function NPCEditorGui::ClickX(%this)//Undo changes to prefs and close //This function is not currently used.
{
   //On exit, undo changes to avatar prefs.
   $Pref::NPCAvatar::Accent = $Old::NPCAvatar::Accent;
   $Pref::NPCAvatar::AccentColor = $Old::NPCAvatar::AccentColor;
   $Pref::NPCAvatar::Chest = $Old::NPCAvatar::Chest;
   $Pref::NPCAvatar::ChestColor = $Old::NPCAvatar::ChestColor;
   $Pref::NPCAvatar::DecalColor = $Old::NPCAvatar::DecalColor;
   $Pref::NPCAvatar::DecalName = $Old::NPCAvatar::DecalName;
   $Pref::NPCAvatar::FaceColor = $Old::NPCAvatar::FaceColor;
   $Pref::NPCAvatar::FaceName = $Old::NPCAvatar::FaceName;
   $Pref::NPCAvatar::Hat = $Old::NPCAvatar::Hat;
   $Pref::NPCAvatar::HatColor = $Old::NPCAvatar::HatColor;
   $Pref::NPCAvatar::Head = $Old::NPCAvatar::Head;
   $Pref::NPCAvatar::HeadColor = $Old::NPCAvatar::HeadColor;
   $Pref::NPCAvatar::Hip = $Old::NPCAvatar::Hip;
   $Pref::NPCAvatar::HipColor = $Old::NPCAvatar::HipColor;
   $Pref::NPCAvatar::LArm = $Old::NPCAvatar::LArm;
   $Pref::NPCAvatar::LArmColor = $Old::NPCAvatar::LArmColor;
   $Pref::NPCAvatar::LHand = $Old::NPCAvatar::LHand;
   $Pref::NPCAvatar::LHandColor = $Old::NPCAvatar::LHandColor;
   $Pref::NPCAvatar::LLeg = $Old::NPCAvatar::LLeg;
   $Pref::NPCAvatar::LLegColor = $Old::NPCAvatar::LLegColor;
   $Pref::NPCAvatar::LSki = $Old::NPCAvatar::LSki;
   $Pref::NPCAvatar::LSkiColor = $Old::NPCAvatar::LSkiColor;
   $Pref::NPCAvatar::Pack = $Old::NPCAvatar::Pack;
   $Pref::NPCAvatar::PackColor = $Old::NPCAvatar::PackColor;
   $Pref::NPCAvatar::RArm = $Old::NPCAvatar::RArm;
   $Pref::NPCAvatar::RArmColor = $Old::NPCAvatar::RArmColor;
   $Pref::NPCAvatar::RHand = $Old::NPCAvatar::RHand;
   $Pref::NPCAvatar::RHandColor = $Old::NPCAvatar::RHandColor;
   $Pref::NPCAvatar::RLeg = $Old::NPCAvatar::RLeg;
   $Pref::NPCAvatar::RLegColor = $Old::NPCAvatar::RLegColor;
   $Pref::NPCAvatar::RSki = $Old::NPCAvatar::RSki;
   $Pref::NPCAvatar::RSkiColor = $Old::NPCAvatar::RSkiColor;
   $Pref::NPCAvatar::SecondPack = $Old::NPCAvatar::SecondPack;
   $Pref::NPCAvatar::SecondPackColor = $Old::NPCAvatar::SecondPackColor;
   $Pref::NPCAvatar::Symmetry = $Old::NPCAvatar::Symmetry;
   $Pref::NPCAvatar::TorsoColor = $Old::NPCAvatar::TorsoColor;

   //Clear our NPC Data values
   NPCData_Name.setValue("");
   NPCData_Job.setSelected(0);
   NPCData_Job.setText("N/A");
   NPCData_Money.setValue(0);
   NPCData_Hunger.setValue(0);
   NPCData_Greed.setValue("$ - ");
   NPCData_Rent_Budget.setValue("0");
   NPCData_Rent_Raycast.setValue("0");
   NPCData_Pose_Frame.clearPoses();
   NPCData_ID.setText("<font:Impact:18><just:center>-");
   NPCData_Section.currentNPC = "";
   //Close gui.
   Canvas.popDialog("NPCEditorGui");
}
function NPCEditorGui::onSleep(%this)
{
   //Remove temp avatar prefs?
   deleteVariables("$Old::NPCAvatar::*");

   %this.addMessage("\c1Editor closed...");

   //Don't leave us stuck orbiting a brick pls.
   commandToServer('RequestResetFromNPCOrbit');
   NPCBrick_Camera.currentOrbit = "";
   $mvYawLeftSpeed = "0";
}
function NPCEditorGui_Done()//Save avatar changes as prefs and close dialog.
{
   Canvas.popDialog(NPCEditorGui);
   export("$pref::*", "config/client/prefs.cs", False);
}

//////////////////
//Avatar Section//
//////////////////

//Don't ask me about this shit.
//It works, I made a few changes to account for hair, the head node, and ski nodes.
function NPCEditorGui_LoadAccentInfo(%arrayName, %filename)
{
   %file = new FileObject(""){};
   %file.openForRead(%filename);
   if (%file.isEOF())
   {
      %file.delete();
      return;
   }
   %file.readLine();
   %line = %file.readLine();
   %wc = getWordCount(%line);
   %i = 0;
   while(%i < %wc)
   {
      %word = getWord(%line, %i);
      %command = "$NPC" @ %arrayName @ "[" @ %i @ "] = \"" @ %word @ "\";";
      eval(%command);
      %i++;
   }
   $numNPC[%arrayName] = %wc;
   %lineCount = 0;
   while(!%file.isEOF())
   {
      %line = %file.readLine();
      %wc = getWordCount(%line);
      %hat = getWord(%line, 0);
      %allowed = getWords(%line, 1, %wc - 1.0);
      %command = "$NPC" @ %arrayName @ "sAllowed[" @ %hat @ "] = \"" @ %allowed @ "\";";
      eval(%command);
      %lineCount++;
   }
   %file.close();
   %file.delete();
}
function NPCEditorGui_CreatePartMenu(%name, %cmdString, %filename, %xPos, %yPos)
{
   if (isObject(%name))
   {
      eval(%name @ ".delete();");
   }
   %newScroll = new GuiScrollCtrl(""){
   };
   %newScroll.vScrollBar = "alwaysOn";
   %newScroll.hScrollBar = "alwaysOff";
   %newScroll.setProfile(ColorScrollProfile);
   NPCAvatar_Section.add(%newScroll);
   %w = 64.0 + 12.0;
   %h = 64;
   %newScroll.resize(%xPos, %yPos, %w, %h);
   %newScroll.setName(%name);
   NPCAvatar_Section.schedule(10, pushToBack, %name);
   %newBox = new GuiBitmapCtrl(""){};
   %newScroll.add(%newBox);
   %newBox.setBitmap("base/client/ui/btnDecalBG");
   %newBox.wrap = 1;
   %newBox.resize(0, 0, 64, 64);
   %newBox.setName("NPCAvatar_" @ fileBase(%filename) @ "MenuBG");
   %file = new FileObject(""){};
   %file.openForRead(%filename);
   %itemCount = 0;
   %iconDir = "Add-Ons/Client_NPCEditor/avatarIcons/" @ fileBase(%filename) @ "/";
   %varString = "$NPC" @ fileBase(%filename);
   %line = %file.readLine();
   while(%line !$= "")
   {
      %newImage = new GuiBitmapCtrl(""){};
      %newBox.add(%newImage);
      %newImage.keepCached = 1;
      %newImage.setBitmap(%iconDir @ %line);
      %x = %itemCount % 4 * 64.0;//fixed
      %y = mFloor(%itemCount / 4.0) * 64.0;
      %newImage.resize(%x, %y, 64, 64);
      %newButton = new GuiBitmapButtonCtrl(""){
         profile = "BlockButtonProfile";
      };
      %newBox.add(%newButton);
      %newButton.setBitmap("base/client/ui/btnDecal");
      %newButton.setText(" ");
      %newButton.resize(%x, %y, 64, 64);
      %newButton.command = %cmdString @ "(" @ %itemCount @ "," @ %newImage @ ");";
      %cmd = %varString @ "[" @ %itemCount @ "] = \"" @ %line @ "\";";
      eval(%cmd);
      %itemCount++;
      %line = %file.readLine();
   }
   $numNPC[fileBase(%filename)] = %itemCount;
   %file.close();
   %file.delete();
   if (%itemCount >= 4.0)
   {
      %w = 4.0 * 64.0;
   }
   else
   {
      %w = %itemCount * 64.0;
   }
   %h = (mFloor(%itemCount / 4.0 + 0.95)) * 64.0;
   %newBox.resize(0, 0, %w, %h);
   if (%yPos + %h > 480.0)
   {
      %h = (mFloor((480.0 - %yPos) / 64.0)) * 64.0;
   }
   %newScroll.resize(%xPos, %yPos, %w + 12.0, %h);
   %newScroll.setVisible(0);
}
function NPCEditorGui_CreatePartMenuFACE(%name, %cmdString, %filename, %xPos, %yPos)
{
   if (isObject(%name))
   {
      eval(%name @ ".delete();");
   }
   %newScroll = new GuiScrollCtrl(""){};
   %newScroll.vScrollBar = "alwaysOn";
   %newScroll.hScrollBar = "alwaysOff";
   %newScroll.setProfile(ColorScrollProfile);
   NPCAvatar_Section.add(%newScroll);
   %w = 64.0 + 12.0;
   %h = 64;
   %newScroll.resize(%xPos, %yPos, %w, %h);
   %newScroll.setName(%name);
   NPCAvatar_Section.schedule(10, pushToBack, %name);
   %newBox = new GuiBitmapCtrl(""){};
   %newScroll.add(%newBox);
   %newBox.setBitmap("base/client/ui/btnDecalBG");
   %newBox.wrap = 1;
   %newBox.resize(0, 0, 64, 64);
   %newBox.setName("NPCAvatar_" @ fileBase(%filename) @ "MenuBG");
   %file = new FileObject(""){};
   %file.openForRead(%filename);
   %itemCount = 0;
   %varString = "$NPC" @ fileBase(%filename);
   %line = %file.readLine();
   while(%line !$= "")
   {
      %newImage = new GuiBitmapCtrl(""){};
      %newBox.add(%newImage);
      %newImage.keepCached = 1;
      %thumbFile = filePath(%line) @ "/thumbs/" @ fileBase(%line);
      %newImage.setBitmap(%thumbFile);
      %x = %itemCount % 4 * 64.0;//fixed
      %y = mFloor(%itemCount / 4.0) * 64.0;
      %newImage.resize(%x, %y, 64, 64);
      %newButton = new GuiBitmapButtonCtrl(""){
         profile = "BlockButtonProfile";
      };
      %newBox.add(%newButton);
      %newButton.setBitmap("base/client/ui/btnDecal");
      %newButton.setText(" ");
      %newButton.resize(%x, %y, 64, 64);
      %newButton.command = %cmdString @ "(" @ %itemCount @ "," @ %newImage @ ");";
      %cmd = %varString @ "[" @ %itemCount @ "] = \"" @ %line @ "\";";
      eval(%cmd);
      %itemCount++;
      %line = %file.readLine();
   }
   $numNPC[fileBase(%filename)] = %itemCount;
   %file.close();
   %file.delete();
   if (%itemCount >= 4.0)
   {
      %w = 4.0 * 64.0;
   }
   else
   {
      %w = %itemCount * 64.0;
   }
   %h = (mFloor(%itemCount / 4.0 + 0.95)) * 64.0;
   %newBox.resize(0, 0, %w, %h);
   if (%yPos + %h > 480.0)
   {
      %h = (mFloor((480.0 - %yPos) / 64.0)) * 64.0;
   }
   %newScroll.resize(%xPos, %yPos, %w + 12.0, %h);
   %newScroll.setVisible(0);
}
function NPCEditorGui_CreateSubPartMenu(%name, %cmdString, %subPartList, %xPos, %yPos)
{
   if (isObject(%name))
   {
      eval(%name @ ".delete();");
   }
   %baseName = strreplace(%name, "Menu", "");
   %baseName = strreplace(%baseName, "NPCAvatar_", "");
   %newScroll = new GuiScrollCtrl(""){};
   %newScroll.vScrollBar = "alwaysOn";
   %newScroll.hScrollBar = "alwaysOff";
   %newScroll.setProfile(ColorScrollProfile);
   NPCAvatar_Section.add(%newScroll);
   %w = 64.0 + 12.0;
   %h = 64;
   %newScroll.resize(%xPos, %yPos, %w, %h);
   %newScroll.setName(%name);
   %newBox = new GuiBitmapCtrl(""){};
   %newScroll.add(%newBox);
   %newBox.setBitmap("base/client/ui/btnDecalBG");
   %newBox.wrap = 1;
   %newBox.resize(0, 0, 64, 64);
   %newBox.setName("NPCAvatar_" @ %baseName @ "MenuBG");
   %iconDir = "Add-Ons/Client_NPCEditor/avatarIcons/" @ %baseName @ "/";
   %itemCount = 0;
   %line = getWord(%subPartList, %itemCount);
   while(%line !$= "")
   {
      %newImage = new GuiBitmapCtrl(""){
      };
      %newBox.add(%newImage);
      %newImage.keepCached = 1;
      %newImage.setBitmap(%iconDir @ %line);
      %x = %itemCount % 4 * 64.0;//fixed
      %y = mFloor(%itemCount / 4.0) * 64.0;
      %newImage.resize(%x, %y, 64, 64);
      %newButton = new GuiBitmapButtonCtrl(""){
      };
      %newBox.add(%newButton);
      %newButton.setBitmap("base/client/ui/btnDecal");
      %newButton.setText(" ");
      %newButton.resize(%x, %y, 64, 64);
      %newButton.command = %cmdString @ "(" @ %itemCount @ "," @ %newImage @ ");";
      %itemCount++;
      %line = getWord(%subPartList, %itemCount);
   }
   if (%itemCount >= 4.0)
   {
      %w = 4.0 * 64.0;
   }
   else
   {
      %w = %itemCount * 64.0;
   }
   %h = (mFloor(%itemCount / 4.0 + 0.95)) * 64.0;
   %newBox.resize(0, 0, %w, %h);
   if (%yPos + %h > 480.0)
   {
      %h = (mFloor((480.0 - %yPos) / 64.0)) * 64.0;
   }
   %newScroll.resize(%xPos, %yPos, %w + 12.0, %h);
   %newScroll.setVisible(0);
}
function NPCEditorGui_CreateColorMenu(%prefString, %colorList, %xPos, %yPos, %symmetryPrefString, %allowTrans)
{
   %rowLimit = 6;
   if (isObject(NPCAvatar_ColorMenu))
   {
      %oldPos = NPCAvatar_ColorMenu.startPos;
      NPCAvatar_ColorMenu.delete();
      if (%oldPos $= (%xPos SPC %yPos))
      {
         return;
      }
   }
   $CurrNPCColorPrefString = %prefString;
   $CurrNPCColorSymmetryPrefString = %symmetryPrefString;
   %newScroll = new GuiScrollCtrl(""){};
   %newScroll.startPos = %xPos SPC %yPos;
   %newScroll.setProfile(ColorScrollProfile);
   %newScroll.vScrollBar = "alwaysOn";
   %newScroll.hScrollBar = "alwaysOff";
   NPCAvatar_Section.add(%newScroll);
   %newScroll.resize(%xPos, %yPos, 32.0 + 12.0, 32);
   %newScroll.setName("NPCAvatar_ColorMenu");
   %newBox = new GuiSwatchCtrl(""){};
   %newScroll.add(%newBox);
   %newBox.setColor("0 0 0 1");
   %newBox.resize(0, 0, 32, 32);
   %itemCount = 0;
   %colorIndex = getWord(%colorList, %count);
   %i = 0;
   while(%i < $NPCAvatar::NumColors)
   {
      %color = $NPCAvatar::Color[%i];
      if (%color $= "")
      {
         %color = "1 1 1 1";
      }
      %alpha = getWord(%color, 3);
      if (%allowTrans || !%allowTrans & %alpha >= 1.0)
      {
         %newSwatch = new GuiSwatchCtrl(""){};
         %newBox.add(%newSwatch);
         %newSwatch.setColor(%color);
         %x = %itemCount % %rowLimit * 32.0;//fixed
         %y = mFloor(%itemCount / %rowLimit) * 32.0;
         %newSwatch.resize(%x, %y, 32, 32);
         %newButton = new GuiBitmapButtonCtrl(""){};
         %newBox.add(%newButton);
         %newButton.setBitmap("base/client/ui/btnColor");
         %newButton.setText(" ");
         %newButton.resize(%x, %y, 32, 32);
         %newButton.command = "NPCAvatar_AssignColor(" @ %i @ ");";
         %itemCount++;
      }
      %i++;
   }
   if (%itemCount >= %rowLimit)
   {
      %w = %rowLimit * 32.0;
   }
   else
   {
      %w = %itemCount * 32.0;
   }
   %h = (mFloor(%itemCount / %rowLimit) + 1.0) * 32.0;
   %newBox.resize(0, 0, %w, %h);
   if (%yPos + %h > 480.0)
   {
      %h = (mFloor((480.0 - %yPos) / 32.0)) * 32.0;
   }
   if(%yPos+%h>getWord(NPCAvatar_Section.extent,1))
   {
      %yPos = getWord(NPCAvatar_Section.extent,1)-%h;
   }
   %newScroll.resize(%xPos, %yPos, %w + 12.0, %h);
}
function NPCAvatar_AssignColor(%index,%color)//Set a variable preference to a variable color.
{
   if(%color $= "")
      %color = $NPCAvatar::Color[%index];
   %cmd = $CurrNPCColorPrefString @ " = \"" @ %color @ "\";";
   eval(%cmd);
   $CurrNPCColorPrefString = "";
   if ($pref::NPCAvatar::Symmetry && $CurrNPCColorSymmetryPrefString !$= "")
   {
      %cmd = $CurrNPCColorSymmetryPrefString @ " = \"" @ %color @ "\";";
      eval(%cmd);
      $CurrNPCColorSymmetryPrefString = "";
   }
   if(isObject(NPCAvatar_ColorMenu))
   {
      NPCAvatar_ColorMenu.schedule(0, delete);
   }
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_ClickTorsoColor()
{
   %x = getWord(NPCAvatar_TorsoColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_TorsoColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::TorsoColor", $NPCnormalColors, %x, %y);
}
function NPCAvatar_ClickPackColor()
{
   %x = getWord(NPCAvatar_PackColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_PackColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::PackColor", $NPCnormalColors, %x, %y);
}
function NPCAvatar_ClickSecondPackColor()
{
   %x = getWord(NPCAvatar_SecondPackColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_SecondPackColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::SecondPackColor", $NPCnormalColors, %x, %y);
}
function NPCAvatar_ClickHatColor()
{
   %x = getWord(NPCAvatar_HatColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_HatColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::HatColor", $NPCnormalColors, %x, %y);
}
function NPCAvatar_ClickAccentColor()
{
   %x = getWord(NPCAvatar_AccentColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_AccentColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::AccentColor", $NPCnormalColors, %x, %y, "", 1);
}
function NPCAvatar_ClickHeadColor()
{
   %x = getWord(NPCAvatar_HeadColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_HeadColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::HeadColor", $NPCnormalColors, %x, %y);
}
function NPCAvatar_ClickHipColor()
{
   %x = getWord(NPCAvatar_HipColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_HipColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::HipColor", $NPCnormalColors, %x, %y);
}
function NPCAvatar_ClickRightLegColor()
{
   %x = getWord(NPCAvatar_RightLegColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_RightLegColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::RLegColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::LLegColor");
}
function NPCAvatar_ClickRightSkiColor()
{
   %x = getWord(NPCAvatar_RightSkiColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_RightSkiColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::RSkiColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::LSkiColor", 1);
}
function NPCAvatar_ClickRightArmColor()
{
   %x = getWord(NPCAvatar_RightArmColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_RightArmColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::RArmColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::LArmColor");
}
function NPCAvatar_ClickRightHandColor()
{
   %x = getWord(NPCAvatar_RightHandColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_RightHandColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::RHandColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::LHandColor");
}
function NPCAvatar_ClickLeftLegColor()
{
   %x = getWord(NPCAvatar_LeftLegColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_LeftLegColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::LLegColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::RLegColor");
}
function NPCAvatar_ClickLeftSkiColor()
{
   %x = getWord(NPCAvatar_LeftSkiColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_LeftSkiColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::LSkiColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::RSkiColor", 1);
}
function NPCAvatar_ClickLeftArmColor()
{
   %x = getWord(NPCAvatar_LeftArmColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_LeftArmColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::LArmColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::RArmColor");
}
function NPCAvatar_ClickLeftHandColor()
{
   %x = getWord(NPCAvatar_LeftHandColor.position, 0) + 32.0;
   %y = getWord(NPCAvatar_LeftHandColor.position, 1);
   NPCEditorGui_CreateColorMenu("$Pref::NPCAvatar::LHandColor", $NPCnormalColors, %x, %y, "$Pref::NPCAvatar::RHandColor");
}
function NPCAvatar_TogglePartMenu(%obj)
{
   %vis = !%obj.visible;
   NPCAvatar_HideAllPartMenus();
   %obj.setVisible(%vis);
}
function NPCAvatar_HideAllPartMenus()
{
   NPCAvatar_FaceMenu.setVisible(0);
   NPCAvatar_DecalMenu.setVisible(0);
   NPCAvatar_PackMenu.setVisible(0);
   NPCAvatar_HatMenu.setVisible(0);
   NPCAvatar_AccentMenu.setVisible(0);
   NPCAvatar_ChestMenu.setVisible(0);
   NPCAvatar_SecondPackMenu.setVisible(0);
   NPCAvatar_ChestMenu.setVisible(0);
   NPCAvatar_HipMenu.setVisible(0);
   NPCAvatar_LArmMenu.setVisible(0);
   NPCAvatar_RArmMenu.setVisible(0);
   NPCAvatar_LHandMenu.setVisible(0);
   NPCAvatar_RHandMenu.setVisible(0);
   NPCAvatar_LLegMenu.setVisible(0);
   NPCAvatar_RLegMenu.setVisible(0);
   if (isObject(NPCAvatar_ColorMenu))
   {
      NPCAvatar_ColorMenu.delete();
   }
}
function NPCAvatar_SetFace(%index, %imageObj)
{
   $Pref::NPCAvatar::FaceName = $NPCface[%index];
   $Pref::NPCAvatar::FaceColor = %index;
   NPCAvatar_FaceMenu.setVisible(0);
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetDecal(%index, %imageObj)
{
   $Pref::NPCAvatar::DecalName = $NPCdecal[%index];
   $Pref::NPCAvatar::DecalColor = %index;
   NPCAvatar_DecalMenu.setVisible(0);
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetPack(%index, %imageObj)
{
   $Pref::NPCAvatar::Pack = %index;
   NPCAvatar_PackMenu.setVisible(0);
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetSecondPack(%index, %imageObj)
{
   $Pref::NPCAvatar::SecondPack = %index;
   NPCAvatar_SecondPackMenu.setVisible(0);
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetHat(%index, %imageObj)
{
   $Pref::NPCAvatar::Hat = %index;
   NPCAvatar_HatMenu.setVisible(0);
   %x = getWord(NPCAvatar_AccentPreview.position, 0) + 64.0;
   %y = getWord(NPCAvatar_AccentPreview.position, 1);
   NPCEditorGui_CreateSubPartMenu("NPCAvatar_AccentMenu", "NPCAvatar_SetAccent", $NPCaccentsAllowed[$NPChat[%index]], %x, %y);
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetAccent(%index, %imageObj)
{
   $Pref::NPCAvatar::Accent = %index;
   NPCAvatar_AccentMenu.setVisible(0);
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetChest(%index)
{
   $Pref::NPCAvatar::Chest = %index;
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetHip(%index)
{
   $Pref::NPCAvatar::Hip = %index;
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetLArm(%index)
{
   $Pref::NPCAvatar::LArm = %index;
   if ($pref::NPCAvatar::Symmetry == 1.0)
   {
      $Pref::NPCAvatar::RArm = %index;
   }
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetRArm(%index)
{
   $Pref::NPCAvatar::RArm = %index;
   if ($pref::NPCAvatar::Symmetry == 1.0)
   {
      $Pref::NPCAvatar::LArm = %index;
   }
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetLHand(%index)
{
   $Pref::NPCAvatar::LHand = %index;
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetRHand(%index)
{
   $Pref::NPCAvatar::RHand = %index;
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetLLeg(%index)
{
   $Pref::NPCAvatar::LLeg = %index;
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_SetRLeg(%index)
{
   $Pref::NPCAvatar::RLeg = %index;
   NPCAvatar_HideAllPartMenus();
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_UpdatePreview()
{
   NPCAvatar_Preview.hideNode("", lski);
   NPCAvatar_Preview.hideNode("", rski);
   NPCAvatar_Preview.hideNode("", HeadSkin);
   %i = 0;
   while(%i < $numNPC["Pack"])
   {
      if ($NPCpack[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCpack[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["SecondPack"])
   {
      if ($NPCSecondPack[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCSecondPack[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["Hat"])
   {
      if ($NPChat[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPChat[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["Accent"])
   {
      if ($NPCAccent[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCAccent[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["Chest"])
   {
      if ($NPCChest[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCChest[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["Hip"])
   {
      if ($NPCHip[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCHip[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["RArm"])
   {
      if ($NPCRArm[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCRArm[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["LArm"])
   {
      if ($NPCLArm[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCLArm[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["RHand"])
   {
      if ($NPCRHand[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCRHand[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["LHand"])
   {
      if ($NPCLHand[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCLHand[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["RLeg"])
   {
      if ($NPCRLeg[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCRLeg[%i]);
      }
      %i++;
   }
   %i = 0;
   while(%i < $numNPC["LLeg"])
   {
      if ($NPCLLeg[%i] !$= "None")
      {
         NPCAvatar_Preview.hideNode("", $NPCLLeg[%i]);
      }
      %i++;
   }
   if ($Pref::NPCAvatar::Pack == 0.0 && $Pref::NPCAvatar::SecondPack == 0.0)
   {
      NPCAvatar_Preview.setThreadPos("", 0, 0);
   }
   else
   {
      NPCAvatar_Preview.setThreadPos("", 0, 1);
   }
   if ($Pref::NPCAvatar::Head)
   {
      NPCAvatar_Preview.unHideNode("","HeadSkin");
      NPCAvatar_Preview.setNodeColor("", "HeadSkin", $Pref::NPCAvatar::HeadColor);
   }
   if ($NPChat[$Pref::NPCAvatar::Hat] !$= "" && $NPChat[$Pref::NPCAvatar::Hat] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPChat[$Pref::NPCAvatar::Hat]);
      NPCAvatar_Preview.setNodeColor("", $NPChat[$Pref::NPCAvatar::Hat], $Pref::NPCAvatar::HatColor);
   }
   %partList = $NPCaccentsAllowed[$NPChat[$Pref::NPCAvatar::Hat]];
   %AccentName = getWord(%partList, $Pref::NPCAvatar::Accent);
   if (%AccentName !$= "" && %AccentName !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", %AccentName);
      NPCAvatar_Preview.setNodeColor("", %AccentName, $Pref::NPCAvatar::AccentColor);
   }
   if ($NPCpack[$Pref::NPCAvatar::Pack] !$= "" && $NPCpack[$Pref::NPCAvatar::Pack] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCpack[$Pref::NPCAvatar::Pack]);
      NPCAvatar_Preview.setNodeColor("", $NPCpack[$Pref::NPCAvatar::Pack], $Pref::NPCAvatar::PackColor);
   }
   if ($NPCSecondPack[$Pref::NPCAvatar::SecondPack] !$= "" && $NPCSecondPack[$Pref::NPCAvatar::SecondPack] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCSecondPack[$Pref::NPCAvatar::SecondPack]);
      NPCAvatar_Preview.setNodeColor("", $NPCSecondPack[$Pref::NPCAvatar::SecondPack], $Pref::NPCAvatar::SecondPackColor);
   }
   if ($NPCChest[$Pref::NPCAvatar::Chest] !$= "" && $NPCChest[$Pref::NPCAvatar::Chest] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCChest[$Pref::NPCAvatar::Chest]);
      NPCAvatar_Preview.setNodeColor("", $NPCChest[$Pref::NPCAvatar::Chest], $Pref::NPCAvatar::TorsoColor);
   }
   if ($NPCHip[$Pref::NPCAvatar::Hip] !$= "" && $NPCHip[$Pref::NPCAvatar::Hip] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCHip[$Pref::NPCAvatar::Hip]);
      NPCAvatar_Preview.setNodeColor("", $NPCHip[$Pref::NPCAvatar::Hip], $Pref::NPCAvatar::HipColor);
   }
   if ($NPCRArm[$Pref::NPCAvatar::RArm] !$= "" && $NPCRArm[$Pref::NPCAvatar::RArm] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCRArm[$Pref::NPCAvatar::RArm]);
      NPCAvatar_Preview.setNodeColor("", $NPCRArm[$Pref::NPCAvatar::RArm], $Pref::NPCAvatar::RArmColor);
   }
   if ($NPCLArm[$Pref::NPCAvatar::LArm] !$= "" && $NPCLArm[$Pref::NPCAvatar::LArm] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCLArm[$Pref::NPCAvatar::LArm]);
      NPCAvatar_Preview.setNodeColor("", $NPCLArm[$Pref::NPCAvatar::LArm], $Pref::NPCAvatar::LArmColor);
   }
   if ($NPCRHand[$Pref::NPCAvatar::RHand] !$= "" && $NPCRHand[$Pref::NPCAvatar::RHand] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCRHand[$Pref::NPCAvatar::RHand]);
      NPCAvatar_Preview.setNodeColor("", $NPCRHand[$Pref::NPCAvatar::RHand], $Pref::NPCAvatar::RHandColor);
   }
   if ($NPCLHand[$Pref::NPCAvatar::LHand] !$= "" && $NPCLHand[$Pref::NPCAvatar::LHand] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCLHand[$Pref::NPCAvatar::LHand]);
      NPCAvatar_Preview.setNodeColor("", $NPCLHand[$Pref::NPCAvatar::LHand], $Pref::NPCAvatar::LHandColor);
   }
   if ($NPCRLeg[$Pref::NPCAvatar::RLeg] !$= "" && $NPCRLeg[$Pref::NPCAvatar::RLeg] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCRLeg[$Pref::NPCAvatar::RLeg]);
      NPCAvatar_Preview.setNodeColor("", $NPCRLeg[$Pref::NPCAvatar::RLeg], $Pref::NPCAvatar::RLegColor);
   }
   if ($Pref::NPCAvatar::RSki)
   {
      NPCAvatar_Preview.unHideNode("","rski");
      NPCAvatar_Preview.setNodeColor("", "rski", $Pref::NPCAvatar::RSkiColor);
   }
   if ($NPCLLeg[$Pref::NPCAvatar::LLeg] !$= "" && $NPCLLeg[$Pref::NPCAvatar::LLeg] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCLLeg[$Pref::NPCAvatar::LLeg]);
      NPCAvatar_Preview.setNodeColor("", $NPCLLeg[$Pref::NPCAvatar::LLeg], $Pref::NPCAvatar::LLegColor);
   }
   if ($Pref::NPCAvatar::LSki)
   {
      NPCAvatar_Preview.unHideNode("","lski");
      NPCAvatar_Preview.setNodeColor("", "lski", $Pref::NPCAvatar::LSkiColor);
   }
   if ($NPCHip[$Pref::NPCAvatar::Hip] !$= "" && $NPCHip[$Pref::NPCAvatar::Hip] !$= "none")
   {
      NPCAvatar_Preview.unHideNode("", $NPCHip[$Pref::NPCAvatar::Hip]);
      NPCAvatar_Preview.setNodeColor("", $NPCHip[$Pref::NPCAvatar::Hip], $Pref::NPCAvatar::HipColor);
      if ($NPCHip[$Pref::NPCAvatar::Hip] $= "skirtHip")
      {
         NPCAvatar_Preview.unHideNode("", "SkirtTrimLeft");
         NPCAvatar_Preview.unHideNode("", "SkirtTrimRight");
         NPCAvatar_Preview.setNodeColor("", "SkirtTrimLeft", $Pref::NPCAvatar::LLegColor);
         NPCAvatar_Preview.setNodeColor("", "SkirtTrimRight", $Pref::NPCAvatar::RLegColor);
         if($NPCRLeg[$Pref::NPCAvatar::RLeg] !$= "None")
            NPCAvatar_Preview.hideNode("", $NPCRLeg[$Pref::NPCAvatar::RLeg]);
         if($NPCLLeg[$Pref::NPCAvatar::LLeg] !$= "None")
            NPCAvatar_Preview.hideNode("", $NPCLLeg[$Pref::NPCAvatar::LLeg]);
      }
      else
      {
         NPCAvatar_Preview.hideNode("", "SkirtTrimLeft");
         NPCAvatar_Preview.hideNode("", "SkirtTrimRight");
      }
   }
   else
   {
      NPCAvatar_Preview.hideNode("", "SkirtTrimLeft");
      NPCAvatar_Preview.hideNode("", "SkirtTrimRight");
   }
   NPCAvatar_Preview.setIflFrame("", decal, $Pref::NPCAvatar::DecalColor);
   NPCAvatar_Preview.setIflFrame("", face, $Pref::NPCAvatar::FaceColor);
   NPCAvatar_TorsoColor.setColor($Pref::NPCAvatar::TorsoColor);
   NPCAvatar_ChestPreview.setColor($Pref::NPCAvatar::TorsoColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_ChestMenuBG, $Pref::NPCAvatar::TorsoColor);
   NPCAvatar_DecalBG.setColor($Pref::NPCAvatar::TorsoColor);
   NPCAvatar_DecalMenuBG.setColor($Pref::NPCAvatar::TorsoColor);
   NPCAvatar_HeadColor.setColor($Pref::NPCAvatar::HeadColor);
   NPCAvatar_HeadBG.setColor($Pref::NPCAvatar::HeadColor);
   NPCAvatar_faceMenuBG.setColor($Pref::NPCAvatar::HeadColor);
   NPCAvatar_HipColor.setColor($Pref::NPCAvatar::HipColor);
   NPCAvatar_HipPreview.setColor($Pref::NPCAvatar::HipColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_HipMenuBG, $Pref::NPCAvatar::HipColor);
   NPCAvatar_LeftArmColor.setColor($Pref::NPCAvatar::LArmColor);
   NPCAvatar_LArmPreview.setColor($Pref::NPCAvatar::LArmColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_LarmMenuBG, $Pref::NPCAvatar::LArmColor);
   NPCAvatar_LeftHandColor.setColor($Pref::NPCAvatar::LHandColor);
   NPCAvatar_LHandPreview.setColor($Pref::NPCAvatar::LHandColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_LHandMenuBG, $Pref::NPCAvatar::LHandColor);
   NPCAvatar_LeftLegColor.setColor($Pref::NPCAvatar::LLegColor);
   NPCAvatar_LLegPreview.setColor($Pref::NPCAvatar::LLegColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_LLegMenuBG, $Pref::NPCAvatar::LLegColor);
   NPCAvatar_LeftSkiColor.setColor($Pref::NPCAvatar::LSkiColor);
   NPCAvatar_RightArmColor.setColor($Pref::NPCAvatar::RArmColor);
   NPCAvatar_RArmPreview.setColor($Pref::NPCAvatar::RArmColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_RarmMenuBG, $Pref::NPCAvatar::RArmColor);
   NPCAvatar_RightHandColor.setColor($Pref::NPCAvatar::RHandColor);
   NPCAvatar_RHandPreview.setColor($Pref::NPCAvatar::RHandColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_RHandMenuBG, $Pref::NPCAvatar::RHandColor);
   NPCAvatar_RightLegColor.setColor($Pref::NPCAvatar::RLegColor);
   NPCAvatar_RLegPreview.setColor($Pref::NPCAvatar::RLegColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_RLegMenuBG, $Pref::NPCAvatar::RLegColor);
   NPCAvatar_RightSkiColor.setColor($Pref::NPCAvatar::RSkiColor);
   NPCAvatar_HatColor.setColor($Pref::NPCAvatar::HatColor);
   NPCAvatar_HatPreview.setColor($Pref::NPCAvatar::HatColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_HatMenuBG, $Pref::NPCAvatar::HatColor);
   NPCAvatar_AccentColor.setColor($Pref::NPCAvatar::AccentColor);
   NPCAvatar_AccentPreview.setColor($Pref::NPCAvatar::AccentColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_AccentMenuBG, $Pref::NPCAvatar::AccentColor);
   NPCAvatar_PackColor.setColor($Pref::NPCAvatar::PackColor);
   NPCAvatar_PackPreview.setColor($Pref::NPCAvatar::PackColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_PackMenuBG, $Pref::NPCAvatar::PackColor);
   NPCAvatar_SecondPackColor.setColor($Pref::NPCAvatar::SecondPackColor);
   NPCAvatar_SecondPackPreview.setColor($Pref::NPCAvatar::SecondPackColor);
   NPCAvatar_ColorAllIcons(NPCAvatar_SecondPackMenuBG, $Pref::NPCAvatar::SecondPackColor);
   %iconDir = "Add-Ons/Client_NPCEditor/avatarIcons/";
   %thumb = filePath($NPCface[$Pref::NPCAvatar::FaceColor]) @ "/thumbs/" @ fileBase($NPCface[$Pref::NPCAvatar::FaceColor]);
   NPCAvatar_FacePreview.setBitmap(%thumb);
   %thumb = filePath($NPCdecal[$Pref::NPCAvatar::DecalColor]) @ "/thumbs/" @ fileBase($NPCdecal[$Pref::NPCAvatar::DecalColor]);
   NPCAvatar_DecalPreview.setBitmap($NPCdecal[$Pref::NPCAvatar::DecalColor]);//intended?
   if ($NPCpack[$Pref::NPCAvatar::Pack] $= "none")
   {
      NPCAvatar_PackPreview.setBitmap(%iconDir @ "none");
   }
   else
   {
      NPCAvatar_PackPreview.setBitmap(%iconDir @ "pack/" @ $NPCpack[$Pref::NPCAvatar::Pack]);
   }
   NPCAvatar_SecondPackPreview.setBitmap(%iconDir @ "secondPack/" @ $NPCSecondPack[$Pref::NPCAvatar::SecondPack]);
   NPCAvatar_HatPreview.setBitmap(%iconDir @ "hat/" @ $NPChat[$Pref::NPCAvatar::Hat]);
   NPCAvatar_ChestPreview.setBitmap(%iconDir @ "chest/" @ $NPCChest[$Pref::NPCAvatar::Chest]);
   NPCAvatar_HipPreview.setBitmap(%iconDir @ "hip/" @ $NPCHip[$Pref::NPCAvatar::Hip]);
   NPCAvatar_LArmPreview.setBitmap(%iconDir @ "Larm/" @ $NPCLArm[$Pref::NPCAvatar::LArm]);
   NPCAvatar_LHandPreview.setBitmap(%iconDir @ "Lhand/" @ $NPCLHand[$Pref::NPCAvatar::LHand]);
   NPCAvatar_LLegPreview.setBitmap(%iconDir @ "Lleg/" @ $NPCLLeg[$Pref::NPCAvatar::LLeg]);
   NPCAvatar_RArmPreview.setBitmap(%iconDir @ "Rarm/" @ $NPCRArm[$Pref::NPCAvatar::RArm]);
   NPCAvatar_RHandPreview.setBitmap(%iconDir @ "Rhand/" @ $NPCRHand[$Pref::NPCAvatar::RHand]);
   NPCAvatar_RLegPreview.setBitmap(%iconDir @ "Rleg/" @ $NPCRLeg[$Pref::NPCAvatar::RLeg]);
   %accentList = $NPCaccentsAllowed[$NPChat[$Pref::NPCAvatar::Hat]];
   %accent = getWord(%accentList, $Pref::NPCAvatar::Accent);
   if (%accent $= "")
   {
      NPCAvatar_AccentPreview.setBitmap(%iconDir @ "none");
   }
   else
   {
      NPCAvatar_AccentPreview.setBitmap(%iconDir @ "accent/" @ %accent);
   }
}
function NPCAvatar_ColorAllIcons(%box, %color)
{
   %count = %box.getCount();
   %i = 0;
   while(%i < %count)
   {
      %obj = %box.getObject(%i);
      if (%obj.getClassName() $= "GuiBitmapCtrl")
         %obj.setColor(%color);
      %i++;
   }
}
function NPCAvatar_Randomize()
{
   $Pref::NPCAvatar::FaceColor = getRandom($numNPCFace - 1.0);
   $Pref::NPCAvatar::FaceName = fileBase($NPCface[$Pref::NPCAvatar::FaceColor]);
   $Pref::NPCAvatar::DecalColor = getRandom($numNPCDecal - 1.0);
   $Pref::NPCAvatar::DecalName = fileBase($NPCdecal[$Pref::NPCAvatar::DecalColor]);
   $Pref::NPCAvatar::Hat = getRandom($numNPCHat - 1.0);
   $Pref::NPCAvatar::Pack = getRandom($numNPCPack - 1.0);
   $Pref::NPCAvatar::SecondPack = getRandom($numNPCSecondPack - 1.0);
   $Pref::NPCAvatar::LArm = getRandom(1,$numNPCLArm - 1.0);
   $Pref::NPCAvatar::RArm = $Pref::NPCAvatar::LArm;
   %chance = getRandom(100);
   %normalHands = 0;
   if (%chance < 70.0)
   {
      %normalHands = 1;
      $Pref::NPCAvatar::LHand = 1;
      $Pref::NPCAvatar::RHand = 1;
   }
   else
   {
      $Pref::NPCAvatar::LHand = getRandom(1,$numNPCLHand - 1.0);
      $Pref::NPCAvatar::RHand = getRandom(1,$numNPCRHand - 1.0);
   }
   %chance = getRandom(100);
   if (%chance < 80.0)
   {
      $Pref::NPCAvatar::LLeg = 1;
      $Pref::NPCAvatar::RLeg = 1;
   }
   else
   {
      $Pref::NPCAvatar::LLeg = getRandom(1,$numNPCLLeg - 1.0);
      $Pref::NPCAvatar::RLeg = getRandom(1,$numNPCRLeg - 1.0);
   }
   $Pref::NPCAvatar::Chest = getRandom(1,$numNPCChest - 1.0);
   %chance = getRandom(100);
   if (%chance < 70.0)
   {
      $Pref::NPCAvatar::Hip = 1;
   }
   else
   {
      $Pref::NPCAvatar::Hip = getRandom(1,$numNPCHip - 1.0);
   }
   if ($NPCChest[$Pref::NPCAvatar::Chest] $= "femchest")
   {
      if ($Pref::NPCAvatar::FaceName !$= "smiley" && $Pref::NPCAvatar::FaceName !$= "smileyCreepy" && strstr(strlwr($Pref::NPCAvatar::FaceName), "female") == -1.0)
      {
         $Pref::NPCAvatar::Chest = 1;
      }
   }
   else
   {
      if (strpos(strlwr($Pref::NPCAvatar::FaceName), "female") != -1.0)
      {
         $Pref::NPCAvatar::Chest = 2;
      }
   }
   %partList = $NPCaccentsAllowed[$NPChat[$Pref::NPCAvatar::Hat]];
   %count = getWordCount(%partList) - 1.0;
   if (%count > 0.0)
   {
      %chance = getRandom(5);
      if (%chance != 0.0)
      {
         $Pref::NPCAvatar::Accent = getRandom(%count - 1.0) + 1.0;
      }
      else
      {
         $Pref::NPCAvatar::Accent = 0;
      }
   }
   else
   {
      $Pref::NPCAvatar::Accent = 0;
   }
   %x = getWord(NPCAvatar_AccentPreview.position, 0) + 64.0;
   %y = getWord(NPCAvatar_AccentPreview.position, 1);
   %hatName = $NPChat[$Pref::NPCAvatar::Hat];
   %AccentArray = $NPCaccentsAllowed[%hatName];
   %AccentName = getWord(%AccentArray, $Pref::NPCAvatar::Accent);
   NPCEditorGui_CreateSubPartMenu("NPCAvatar_AccentMenu", "NPCAvatar_SetAccent", %AccentArray, %x, %y);
   %count = getWordCount(%torsoColorList) - 1.0;
   $Pref::NPCAvatar::TorsoColor = NPCAvatar_GetRandomColor(0);
   %count = getWordCount(%packColorList) - 1.0;
   $Pref::NPCAvatar::PackColor = NPCAvatar_GetRandomColor(0);
   $Pref::NPCAvatar::SecondPackColor = NPCAvatar_GetRandomColor(0);
   %count = getWordCount(%hatColorList) - 1.0;
   $Pref::NPCAvatar::HatColor = NPCAvatar_GetRandomColor(0);
   %count = getWordCount(%accentColorList) - 1.0;
   $Pref::NPCAvatar::AccentColor = NPCAvatar_GetRandomColor(1);
   %count = getWordCount(%hipColorList) - 1.0;
   $Pref::NPCAvatar::HipColor = NPCAvatar_GetRandomColor(0);
   %count = getWordCount(%LLegColorList) - 1.0;
   $Pref::NPCAvatar::LLegColor = NPCAvatar_GetRandomColor(0);
   %count = getWordCount(%LArmColorList) - 1.0;
   $Pref::NPCAvatar::LArmColor = NPCAvatar_GetRandomColor(0);
   %count = getWordCount(%LHandColorList) - 1.0;
   $Pref::NPCAvatar::LHandColor = NPCAvatar_GetRandomColor(0);
   if (%normalHands)
   {
      if (getRandom(1))
      {
         $Pref::NPCAvatar::LHandColor = $Pref::NPCAvatar::HeadColor;
      }
   }
   if ($pref::NPCAvatar::Symmetry == 1.0)
   {
      $Pref::NPCAvatar::RLegColor = $Pref::NPCAvatar::LLegColor;
      $Pref::NPCAvatar::RArmColor = $Pref::NPCAvatar::LArmColor;
      $Pref::NPCAvatar::RHandColor = $Pref::NPCAvatar::LHandColor;
   }
   else
   {
      %count = getWordCount(%RLegColorList) - 1.0;
      $Pref::NPCAvatar::RLegColor = NPCAvatar_GetRandomColor(0);
      %count = getWordCount(%RArmColorList) - 1.0;
      $Pref::NPCAvatar::RArmColor = NPCAvatar_GetRandomColor(0);
      %count = getWordCount(%RHandColorList) - 1.0;
      $Pref::NPCAvatar::RHandColor = NPCAvatar_GetRandomColor(0);
   }
   NPCAvatar_UpdatePreview();
}
function NPCAvatar_GetRandomColor(%allowTrans)
{
   %i = 0;
   while(%i < 1000.0)
   {
      %color = $NPCAvatar::Color[getRandom(8)];
      %alpha = getWord(%color, 3);
      if (%allowTrans)
      {
         return %color;
      }
      else
      {
         if (%alpha >= 1.0)
         {
            return %color;
         }
      }
      %i++;
   }
   return %color;
}
function NPCAvatar_Clean()//Removes Script-created gui elements, to make the gui easier to save.
{
   NPCAvatar_AccentMenu.delete();
   NPCAvatar_FaceMenu.delete();
   NPCAvatar_DecalMenu.delete();
   NPCAvatar_PackMenu.delete();
   NPCAvatar_SecondPackMenu.delete();
   NPCAvatar_HatMenu.delete();
   NPCAvatar_ChestMenu.delete();
   NPCAvatar_HipMenu.delete();
   NPCAvatar_RLegMenu.delete();
   NPCAvatar_LLegMenu.delete();
   NPCAvatar_RArmMenu.delete();
   NPCAvatar_LArmMenu.delete();
   NPCAvatar_RHandMenu.delete();
   NPCAvatar_LHandMenu.delete();
   if(isObject(NPCAvatar_ColorMenu))
      NPCAvatar_ColorMenu.delete();
}
function NPCEditorGui::clickSetFavs(%this)
{
   NPCED_FavsHelper.setVisible(!NPCED_FavsHelper.isVisible());
}
function NPCEditorGui::ClickFav(%this, %idx)
{
   %idx = mFloor(%idx);
   %filename = "config/client/NPCAvatarFavorites/" @ %idx @ ".cs";
   if (NPCED_FavsHelper.isVisible())
   {
      export("$Pref::NPCAvatar::*", %filename, 0);
      NPCED_FavsHelper.setVisible(0);
   }
   else
   {
      if (!isFile(%filename))
      {
         return;
      }
      exec(%filename);
      %i = 0;
      while(%i < $numNPCDecal)
      {
         if (fileBase($NPCdecal[%i]) $= fileBase($Pref::NPCAvatar::DecalName))
            $Pref::NPCAvatar::DecalColor = %i;
         %i++;
      }
      %i = 0;
      while(%i < $numNPCFace)
      {
         if (fileBase($NPCface[%i]) $= fileBase($Pref::NPCAvatar::FaceName))
            $Pref::NPCAvatar::FaceColor = %i;
         %i++;
      }
      NPCAvatar_UpdatePreview();
      $pref::NPCAvatar::Symmetry = NPCAvatar_SymmetryCheckbox.getValue();
   }
   NPCEditorGui.updateFavButtons();
}
function NPCEditorGui::updateFavButtons()
{
   %i = 0;
   while(%i < 37)
   {
      %filename = "config/client/NPCAvatarFavorites/" @ %i @ ".cs";
      if (isFile(%filename))
      {
         eval("NPCAvatar_FavButton" @ %i @ ".setColor(\"1 1 1 1\");");
      }
      else
      {
         eval("NPCAvatar_FavButton" @ %i @ ".setColor(\"1 1 1 0.5\");");
      }
      %i++;
   }
}

//////////////////////////////////////
//NONStandard NPCEditorGui functions//
//////////////////////////////////////

function NPCEditorGui::addMessage(%this, %line)//Add a message to the messagebox on the bottom of the gui.
{
   if(!isObject(%vector = NPCMessage_Vector))
      return;
   while(%vector.getNumLines()>=%vector.maxLines)//Manage the number of lines stored on the message vector.
   {
      %vector.popFrontLine();
   }
   %vector.pushBackLine("\c1" @ getSubStr(getWord(getDateTime(),1),0,5) @ "\c0" SPC %line,0);//Adds a time string and some formatting to the beginning of each message.
}
function NPCEditorGui::buildEventList(%this)//Build a name indexed list of events for use when applying NPC data.
{
   //Clear old IDs
   %this.event["Input","setNPCData","IDx"] = "";
   %this.event["Input","setNPCData","Target","And","IDx"] = "";
   %this.event["Output","setNPCName","IDx"] = "";
   %this.event["Output","setJob","IDx"] = "";
   %this.event["Output","setMoney","IDx"] = "";
   %this.event["Output","setHunger","IDx"] = "";
   %this.event["Output","setGreed","IDx"] = "";
   %this.event["Output","setRent","IDx"] = "";
   %this.event["Output","setFootNodes","IDx"] = "";
   %this.event["Output","setBodyNodes","IDx"] = "";
   %this.event["Output","setArmNodes","IDx"]  ="";
   %this.event["Output","setHeadNodes","IDx"] = "";
   %this.event["Output","setFootColors","IDx"] = "";
   %this.event["Output","setBodyColors","IDx"] = "";
   %this.event["Output","setArmColors","IDx"] = "";
   %this.event["Output","setHeadColors","IDx"] = "";
   %this.event["Output","setDecals","IDx"] = "";
   %this.event["Output","setPose","IDx"] = "";
   %this.event["Output","Finish","IDx"] = "";

   //Find the Input Event and Target we're gonna use.
   for(%i=0;%i<$Input["Event","CountfxDTSBrick"];%i++)
   {
      if($Input["Event","NamefxDTSBrick",%i] $= "setNPCData")
      {
         %this.event["Input","setNPCData","IDx"] = %i;
         %count = getFieldCount($Input["Event","TargetListfxDTSBrick",%i]);
         for(%t=0;%t<%count;%t++)
         {
            %word = getWord(getField($Input["Event","TargetListfxDTSBrick",%i],%t),0);
            if(%word $= "And")
            {
               %this.event["Input","setNPCData","Target","And","IDx"] = %t;
               break;
            }
            if(%t+1==%count)//event not found so return error.
               return 0;
         }
         break;
      }
      if(%i+1==$Input["Event","CountfxDTSBrick"])//event not found so return error;
         return 0;
   }

   //Find all the Output events.
   for(%i=0;%i<$Output["Event","CountNPCSpawn"];%i++)
   {
      switch$ ($Output["Event","NameNPCSpawn",%i])
      {
         case "setNPCName":
            %this.event["Output","setNPCName","IDx"] = %i;
            %numOutputs++;
         case "setJob":
            %this.event["Output","setJob","IDx"] = %i;
            %numOutputs++;
         case "setMoney":
            %this.event["Output","setMoney","IDx"] = %i;
            %numOutputs++;
         case "setHunger":
            %this.event["Output","setHunger","IDx"] = %i;
            %numOutputs++;
         case "setGreed":
            %this.event["Output","setGreed","IDx"] = %i;
            %numOutputs++;
         case "setRent":
            %this.event["Output","setRent","IDx"] = %i;
            %numOutputs++;
         case "setFootNodes":
            %this.event["Output","setFootNodes","IDx"] = %i;
            %numOutputs++;
         case "setBodyNodes":
            %this.event["Output","setBodyNodes","IDx"] = %i;
            %numOutputs++;
         case "setArmNodes":
            %this.event["Output","setArmNodes","IDx"] = %i;
            %numOutputs++;
         case "setHeadNodes":
            %this.event["Output","setHeadNodes","IDx"] = %i;
            %numOutputs++;
         case "setFootColors":
            %this.event["Output","setFootColors","IDx"] = %i;
            %numOutputs++;
         case "setBodyColors":
            %this.event["Output","setBodyColors","IDx"] = %i;
            %numOutputs++;
         case "setArmColors":
            %this.event["Output","setArmColors","IDx"] = %i;
            %numOutputs++;
         case "setHeadColors":
            %this.event["Output","setHeadColors","IDx"] = %i;
            %numOutputs++;
         case "setDecals":
            %this.event["Output","setDecals","IDx"] = %i;
            %numOutputs++;
         case "setPose":
            %this.event["Output","setPose","IDx"] = %i;
            %numOutputs++;
         case "Finish":
            %this.event["Output","Finish","IDx"] = %i;
            %numOutputs++;
      }
   }
   if(%numOutputs<16)//This was actually useful.  I misspelled something while coding it so this made the gui freak out when it couldn't find the right number of events. >.<
      return 0;

   %this.builtEventList = true;
   return 1;
}
function NPCEditorGui::Apply(%this)//Send a small bucket of events to the server containing the NPC Data.
{
   if(NPCData_Section.currentNPC $= "")
      return;
   if(NPCData_Section.receivingData)
   {
      %this.addMessage("\c2Error: Can not Apply.  Still receiving data.");
      return;
   }
   if(!%this.builtEventList)
   {
      %this.addMessage("\c1Building quick event IDx list...");
      if(!%this.buildEventList())
      {
         %this.addmessage("\c2Error: Missing events! Is the server running CityRPG 4? Was there an update we missed?");
         return;
      }
      //%this.addMessage("\c1Built quick eventIDx list.");
   }

   //commandToServer('ClearEvents');
   //commandToServer('addEvent', %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);

   commandToServer('NPCsetWrenchBrick',NPCData_Section.currentNPC);
   commandToServer('ClearEvents');

   //This mess is why we build the event list.
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setNPCName","IDx"], NPCData_Name.getValue(), "", "", "");
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setJob","IDx"], NPCData_Job.getSelected(), "", "", "");
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setMoney","IDx"], NPCData_Money.getValue(), "", "", "");
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setHunger","IDx"], NPCData_Hunger.getValue(), "", "", "");
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setGreed","IDx"], getField(NPCData_Greed.parameters,0), getField(NPCData_Greed.parameters,1), getField(NPCData_Greed.parameters,2), getField(NPCData_Greed.parameters,3));
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setRent","IDx"], NPCData_Rent_Budget.getValue(), NPCData_Rent_Raycast.getValue(), "", "");
   //The avatar data was surprisingly easy to get a hold of, because it's all stored in global variables.
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setFootNodes","IDx"], $Pref::NPCAvatar::LLeg, $Pref::NPCAvatar::RLeg, $Pref::NPCAvatar::LSki, $Pref::NPCAvatar::RSki);
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setBodyNodes","IDx"], $Pref::NPCAvatar::Hip, $Pref::NPCAvatar::Chest, $Pref::NPCAvatar::Pack, $Pref::NPCAvatar::SecondPack);
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setArmNodes","IDx"], $Pref::NPCAvatar::LArm, $Pref::NPCAvatar::RArm, $Pref::NPCAvatar::LHand, $Pref::NPCAvatar::RHand);
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setHeadNodes","IDx"], $Pref::NPCAvatar::Head, $Pref::NPCAvatar::Hat, $Pref::NPCAvatar::Accent, "");
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setFootColors","IDx"], $Pref::NPCAvatar::LLegColor, $Pref::NPCAvatar::RLegColor, $Pref::NPCAvatar::LSkiColor, $Pref::NPCAvatar::RSkiColor);
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setBodyColors","IDx"], $pref::NPCAvatar::HipColor, $pref::NPCAvatar::TorsoColor, $pref::NPCAvatar::PackColor, $pref::NPCAvatar::SecondPackColor);
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setArmColors","IDx"], $pref::NPCAvatar::LArmColor, $pref::NPCAvatar::RArmColor, $pref::NPCAvatar::LHandColor, $pref::NPCAvatar::RHandColor);
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setHeadColors","IDx"], $pref::NPCAvatar::HeadColor, $pref::NPCAvatar::HatColor, $pref::NPCAvatar::AccentColor, "");
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setDecals","IDx"], $Pref::NPCAvatar::FaceColor, $Pref::NPCAvatar::DecalColor, "", "");
   commandToServer('NPCclearWrenchBrick',NPCData_Section.currentNPC);

   //setPoses
   for(%i=0;%i<NPCData_Pose_Frame.numPoses;%i++)
   {
      %hours = nameToID("NPCData_Pose_"@%i@"_Time_H").getValue();
      %minutes = nameToID("NPCData_Pose_"@%i@"_Time_M").getValue();
      %position = nameToID("NPCData_Pose_"@%i@"_Position").getValue();
      %rotation = nameToID("NPCData_Pose_"@%i@"_Rotation").getValue();
      %doAnim = nameToID("NPCData_Pose_"@%i@"_DoAnim").getValue();
      %animation = nameToID("NPCData_Pose_"@%i@"_Animation").getValue();
      commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","setPose","IDx"], %hours@":"@%minutes, %position SPC %rotation, %doAnim, %animation);
   }

   //Don't forget the finish event, or the server won't process our NPC.
   commandToServer('addEvent', 1, %this.event["Input","setNPCData","IDx"], 0, %this.event["Input","setNPCData","Target","And","IDx"], "", %this.event["Output","Finish","IDx"], "", "", "", "");

   %this.addMessage("\c0Sent data for "@NPCData_Name.getValue()@" ("@NPCData_Section.currentNPC@")");
}

////////////////////
//Client Responses//
////////////////////

function clientCmdreceiveSpawnList(%msg,%page,%pages)//Build a book of spawns, then add them to the brick list.
{
   if(!isObject(%list = NPCBrick_List))
      return;
   if(!%list.receiving)
   {
      %list.receiving = true;
      NPCEditorGui.addMessage("\c1Receiving spawn list...");
   }

   %list.page[%page] = %msg;
   if(%page==%pages)//We finished?
   {
      NPCEditorGui.addMessage("\c1Received spawn list. ("@%pages@" pages)");
      %list.receiving = false;
      %list.clearNPCSpawns();//Clear the old list.

      for(%i=1;%i<=%pages;%i++)
      {
         %records = getRecordCount(%list.page[%i]);
         for(%r=0;%r<%records;%r++)
         {
            %record = getRecord(%list.page[%i],%r);
            %list.addNPCSpawn(getField(%record,0),getField(%record,1),getField(%record,2),getField(%record,3));//Add our brick to the list.
         }
      }
      if(%list.selectOnDone)
      {
         %list.selectBrick(%list.selectOnDone);//selectNPCSpawn will wait for us to finish receiving.  Let's finish their job.
      }
      else
      {
         %list.selectBrick(NPCData_Section.currentNPC,1);//Reselect whatever we had selected before.  But only visually (thats what the "1" does).
      }
   }
}
function clientCmdselectNPCSpawn(%brick)//We were looking at an NPC Spawn when we opened the GUI.  Do we select it now or later?
{
   if(!isObject(%list = NPCBrick_List))
      return;
   if(%list.receiving)
   {
      %list.selectOnDone = %brick;
   }
   else
   {
      %list.selectBrick(%brick);
   }
}
function clientcmdreceiveNPCData(%brick,%category,%parameters)//Receive various bits of data from the server and put them in the right place.
{
   if(!isObject(NPCEditorGui))
      return;
   //NPCEditorGui.addMessage("\c1Received NPC Data "@%category@": "@%parameters);
   switch$ (%category)
   {
      case "JOBLIST":
         if(NPCData_Job.listedBrick!=%brick)//The joblist is sent piece by piece.  .listedbrick helps determine if we are still receiving the same list.  It's a bad method.
         {
            NPCData_Job.clear();
            NPCData_Job.listedBrick = %brick;
         }
         NPCData_Job.add(getField(%parameters,0),getField(%parameters,1),0);
      case "NAME":
         NPCData_Name.setValue(%parameters);
         NPCData_Section.dataCount++;
      case "JOB":
         NPCData_Job.setSelected(%parameters);
         NPCData_Section.dataCount++;
      case "MONEY":
         NPCData_Money.setValue(%parameters);
         NPCData_Section.dataCount++;
      case "HUNGER":
         NPCData_Hunger.setValue(%parameters);
         NPCData_Section.dataCount++;
      case "GREED"://We don't modify greed, it is set automatically by the server
         NPCData_Greed.setText("$"@getField(%parameters,0)@" - "@getField(%parameters,1));//But we may wish to view it.
         NPCData_Greed.parameters = getFields(%parameters,2,5);//Store the full set of info so we can send it back later.
         NPCData_Section.dataCount++;
      case "RENT":
         NPCData_Rent_Budget.setValue(getField(%parameters,0));
         NPCData_Rent_Raycast.setValue(getField(%parameters,1));
         NPCData_Section.dataCount++;
      case "FEETN":
         NPCAvatar_SetLLeg(getField(%parameters,0));
         NPCAvatar_SetRLeg(getField(%parameters,1));
         if($Pref::NPCAvatar::LSki != getField(%parameters,2))
            NPCAvatar_LSki.performClick();
         if($Pref::NPCAvatar::RSki != getField(%parameters,3))
            NPCAvatar_RSki.performClick();
         NPCData_Section.dataCount++;
      case "BODYN":
         NPCAvatar_SetHip(getField(%parameters,0));
         NPCAvatar_SetChest(getField(%parameters,1));
         NPCAvatar_SetPack(getField(%parameters,2));
         NPCAvatar_SetSecondPack(getField(%parameters,3));
         NPCData_Section.dataCount++;
      case "ARMN":
         NPCAvatar_SetLArm(getField(%parameters,0));
         NPCAvatar_SetRArm(getField(%parameters,1));
         NPCAvatar_SetLHand(getField(%parameters,2));
         NPCAvatar_SetRHand(getField(%parameters,3));
         NPCData_Section.dataCount++;
      case "HEADN":
         if($Pref::NPCAvatar::Head != getField(%parameters,0))
            NPCAvatar_Head.performClick();
         NPCAvatar_SetHat(getField(%parameters,1));
         NPCAvatar_SetAccent(getField(%parameters,2));
         NPCData_Section.dataCount++;
      case "FEETC":
         $CurrNPCColorPrefString = "$Pref::NPCAvatar::LLegColor";
         NPCAvatar_AssignColor(0,getField(%parameters,0));
         $CurrNPCColorPrefString = "$Pref::NPCAvatar::RLegColor";
         NPCAvatar_AssignColor(0,getField(%parameters,1));
         $CurrNPCColorPrefString = "$Pref::NPCAvatar::LSkiColor";
         NPCAvatar_AssignColor(0,getField(%parameters,2));
         $CurrNPCColorPrefString = "$Pref::NPCAvatar::RSkiColor";
         NPCAvatar_AssignColor(0,getField(%parameters,3));
         NPCData_Section.dataCount++;
      case "BODYC":
         $CurrNPCColorPrefString = "$pref::NPCAvatar::HipColor";
         NPCAvatar_AssignColor(0,getField(%parameters,0));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::TorsoColor";
         NPCAvatar_AssignColor(0,getField(%parameters,1));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::PackColor";
         NPCAvatar_AssignColor(0,getField(%parameters,2));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::SecondPackColor";
         NPCAvatar_AssignColor(0,getField(%parameters,3));
         NPCData_Section.dataCount++;
      case "ARMC":
         $CurrNPCColorPrefString = "$pref::NPCAvatar::LArmColor";
         NPCAvatar_AssignColor(0,getField(%parameters,0));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::RArmColor";
         NPCAvatar_AssignColor(0,getField(%parameters,1));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::LHandColor";
         NPCAvatar_AssignColor(0,getField(%parameters,2));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::RHandColor";
         NPCAvatar_AssignColor(0,getField(%parameters,3));
         NPCData_Section.dataCount++;
      case "HEADC":
         $CurrNPCColorPrefString = "$pref::NPCAvatar::HeadColor";
         NPCAvatar_AssignColor(0,getField(%parameters,0));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::HatColor";
         NPCAvatar_AssignColor(0,getField(%parameters,1));
         $CurrNPCColorPrefString = "$pref::NPCAvatar::AccentColor";
         NPCAvatar_AssignColor(0,getField(%parameters,2));
         NPCData_Section.dataCount++;
      case "DECALS":
         NPCAvatar_SetFace(getField(%parameters,0));
         NPCAvatar_SetDecal(getField(%parameters,1));
         NPCData_Section.dataCount++;
      case "POSE":
         NPCData_Pose_Frame.addPoseRow();
         %poseNum = NPCData_Pose_Frame.numPoses-1;
         nameToID("NPCData_Pose_"@%poseNum@"_Time_H").setValue(getSubStr(getField(%parameters,0),0,2));
         nameToID("NPCData_Pose_"@%poseNum@"_Time_M").setValue(getSubStr(getField(%parameters,0),3,2));
         nameToID("NPCData_Pose_"@%poseNum@"_Position").setValue(getWords(getField(%parameters,1),0,2));
         nameToID("NPCData_Pose_"@%poseNum@"_Rotation").setValue(getWords(getField(%parameters,1),3,6));
         nameToID("NPCData_Pose_"@%poseNum@"_DoAnim").setValue(getField(%parameters,2));
         nameToID("NPCData_Pose_"@%poseNum@"_Animation").setSelected(nameToID("NPCData_Pose_"@%poseNum@"_Animation").findText(getField(%parameters,3)));
         NPCData_Section.dataCount++;
      case "PLAYERPOS":
         nameToID("NPCData_Pose_"@getField(%parameters,1)@"_Position").setValue(getField(%parameters,0));
         NPCData_Section.dataCount++;
      case "PLAYERROT":
         nameToID("NPCData_Pose_"@getField(%parameters,1)@"_Rotation").setValue(getField(%parameters,0));
         NPCData_Section.dataCount++;
      case "FINISH":
         NPCEditorGui.addMessage("\c1Received data for \""@NPCData_Name.getValue()@"\" ("@%brick@")");
         NPCData_ID.setText("<font:Impact:18><just:center>"@%brick);
         NPCData_Section.receivingData = false;
         if(%parameters>3)//This is hacky but- Refresh requests only 3 fields, don't redo the camera work. >.<
         {
            commandToServer('RequestOrbitNPCSpawn',%brick);
            NPCBrick_Camera.currentOrbit = %brick;
            if($mvYawLeftSpeed != "0.0035")
            {
               schedule(140,"NPCEditorGui","eval","$mvYawLeftSpeed = 0.0035;");
               schedule(100,"NPCEditorGui","eval","$mvPitch = -$pi;");
               schedule(140,"NPCEditorGui","eval","$mvPitch = ($pi/8)*5;");
            }
         }
         if(NPCData_Section.dataCount!=%parameters)//We didn't receive all the data, or we received more that we were supposed to.
         {
            NPCEditorGui.addMessage("\c2Error: Bad data count! Did we lose something?");
            NPCEditorGui.addMessage("\c2Tell Dglider.  He's gonna cry, (cause he has to rewrite this whole fucking thing), but tell him.");//I hope you never see this message.
         }
         NPCData_Section.dataCount = 0;
      default:
         NPCEditorGui.addMessage("\c2Received NPC Data "@%category@": "@%parameters);
         NPCEditorGui.addMessage("\c2Error: Invalid NPCData category.  What happened?");
         NPCData_Section.dataCount++;
   }
}
function clientCmdNPCEditorDisplayMsg(%msg)//We got a message from the server!
{
   if(isObject(NPCEditorGui))
   {
      NPCEditorGui.addMessage("\c4Server: "@%msg);
   }
}

////////////////////////
//Brick list functions//
////////////////////////

function NPCBrick_List::requestNPCSpawnList(%this)//Request that the server send a spawn list.  Wait up to 5040 ms if necessary.
{
   cancel(%this.requestNPCSpawnListSched);

   if((%timesince = (getSimTime()-%this.requestNPCSpawnListTimeout))<5040)
   {
      %timetill = 5040 - %timesince;
      NPCEditorGui.addMessage("\c1Requesting npc spawn list in "@%timetill@" ms.");
      %this.requestNPCSpawnListSched = %this.schedule(%timetill,"requestNPCSpawnList");
      return;
   }

   %this.requestNPCSpawnListTimeout = getSimTime();
   NPCEditorGui.addMessage("\c1Requesting npc spawn list...");
   commandToServer('requestNPCSpawnList');
   %this.receiving = true;
}
function NPCBrick_List::clearNPCSpawns(%this)//Clear the brick list and do some kerfuffly because ScrollCtrls suck ass and don't set their extent correctly.
{
   %this.clear();
   %this.addRow(0,"",0);
   %label = NPCBrick_Labels;
   %sizex = getWord((getWord(%this.extent,0)>getWord(%label.extent,0)?%this.extent:%label.extent),0);
   %sizey = getWord((getWord(%this.extent,1)>getWord(%label.extent,1)?%this.extent:%label.extent),1);
   %this.extent = %sizex SPC %sizey;
   %this.setVisible(1);//Refresh parent.
}
function NPCBrick_List::addNPCSpawn(%this,%brickID,%NPCName,%ownerName,%brickPosition)//Add an npc spawn and once again, do some bullshit cause ScrollCtrls suck.
{
   %this.addRow(%brickID,%NPCName TAB %ownerName TAB %brickPosition TAB %brickID, %this.rowCount());
   %label = NPCBrick_Labels;
   %sizex = getWord((getWord(%this.extent,0)>getWord(%label.extent,0)?%this.extent:%label.extent),0);
   %sizey = getWord((getWord(%this.extent,1)>getWord(%label.extent,1)?%this.extent:%label.extent),1);
   %this.extent = %sizex SPC %sizey;
   %this.setVisible(1);//Refresh parent.
}
function NPCBrick_List::sortBrickList(%this,%num,%column,%inc)//Sort the brick list by a column.
{
   %this.removeRow(0);//Row 0 is blank, so remove it temporarily while sorting.  Else it will end up in a bad place.
   if(%num)
      %this.sortNumerical(%column,%inc);
   else
      %this.sort(%column,%inc);
   %this.addRow(0,"",0);
}
function NPCBrick_List::selectBrick(%this,%brick,%noRequest)//Select a brick through script
{
   %this.selectOnDone = "";
   %row = %this.getRowNumByID(%brick);
   if(%row<=0)
      return;
   %this.noRequest = %noRequest;//We can't pass this field on to onSelect directly so....
   %this.setSelectedRow(%row);
}
function NPCBrick_List::onSelect(%this,%str)//When a brick is selected, through script or with the mouse.
{
   %noRequest = %this.noRequest;//Retrieve the noRequest variable.
   %this.noRequest = "";

   %brick = %this.getSelectedID();
   %row = %this.getRowNumByID(%brick);
   %text = %this.getRowText(%row);
   if(%text$="")//they somehow selected the line below the buttons >.<
      return;
   if(!%noRequest)
   {
      NPCEditorGui.addMessage("Selected NPC \""@getField(%text,0)@"\"");
      NPCData_Section.requestNPCData(%brick, getField(%text,0));
   }
}

///////////////////
//Camera function//
///////////////////

function NPCBrick_Camera::MoveToOrbit(%this)//Request a teleport and close the GUI
{
   if(%this.currentOrbit!$="")
   {
      commandToServer('RequestTPToOrbit',%this.currentOrbit);
   }
   NPCEditorGui_Done();
}

//////////////////
//Data functions//
//////////////////

function NPCData_Section::requestNPCData(%this,%brick,%name)//Request data for a specific NPC  Timeout 1040 ms
{
   cancel(%this.requestNPCDataSched);

   if((%timesince = (getSimTime()-%this.requestNPCDataTimeout))<1040)
   {
      %timetill = 1040 - %timesince;
      NPCEditorGui.addMessage("\c1Requesting npc data in "@%timetill@" ms.");
      %this.requestNPCDataSched = %this.schedule(%timetill,"requestNPCData",%brick,%name);
      return;
   }

   %this.requestNPCDataTimeout = getSimTime();
   %this.currentNPC = %brick;
   %this.receivingData = true;
   %this.dataCount = 0;
   NPCData_Pose_Frame.clearPoses();
   NPCData_ID.setText("<font:Impact:18><just:center>-");
   commandToServer('requestNPCData',%brick);
   NPCEditorGui.addMessage("\c1Requesting data for \""@%name@"\" ("@%brick@")");
}
function NPCData_Section::requestNPCDataRefresh(%this,%brick)//Request just a few fields.  Timeour 1040 ms
{
   cancel(%this.requestNPCDataSched);

   if((%timesince = (getSimTime()-%this.requestNPCDataTimeout))<1040)
   {
      %timetill = 1040 - %timesince;
      NPCEditorGui.addMessage("\c1Refreshing npc data in "@%timetill@" ms.");
      %this.requestNPCDataSched = %this.schedule(%timetill,"requestNPCDataRefresh",%brick);
      return;
   }

   if(!%this.receivingData)
   {
      %this.requestNPCDataTimeout = getSimTime();
      NPCEditorGui.addMessage("\c1Refreshing money, hunger, and greed.");
      %this.receivingData = true;
      commandToServer('requestNPCDataRefresh',%brick);
   }
}
function NPCData_Section::fillRent(%this)//Request that the server send you a valid rent raycast based on your player aim.
{
   cancel(%this.requestNPCDataSched);

   if(%this.currentNPC $= "")
      return;

   if((%timesince = (getSimTime()-%this.requestNPCDataTimeout))<540)
   {
      %timetill = 540 - %timesince;
      NPCEditorGui.addMessage("\c1Requesting player aim for rent in "@%timetill@" ms.");
      %this.requestNPCDataSched = %this.schedule(%timetill,"fillRent");
      return;
   }

   if(!%this.receivingData)
   {
      commandToServer('RequestResetFromNPCOrbit');
      NPCBrick_Camera.currentOrbit = "";
      $mvYawLeftSpeed = "0";

      %this.requestNPCDataTimeout = getSimTime();
      NPCEditorGui.addMessage("\c1Requesting player aim for rent...");
      %this.receivingData = true;
      commandToServer('RequestFillRent',%this.currentNPC);
   }
}

////////////////////////
//Pose Frame functions//
////////////////////////

function NPCData_Pose_Frame::clearPoses(%this)//Erase everything in the frameset and hide.
{
   for(%i=0;%i<%this.numPoses;%i++)
   {
      %this.PosButt[%i].delete();
      %this.RotButt[%i].delete();
   }
   %this.deleteAll();
   NPCData_Pose_AddRow.position = "3 2";
   %this.numPoses = 0;
   %this.setVisible(0);
}
function NPCData_Pose_Frame::addPoseRow(%this)//Add all of the boxes and buttons for a pose
{
   %this.numPoses += 1;

   //Adjust extent and rows
   %this.extent = setWord(%this.extent,1,%this.numPoses*18+(%this.numPoses-1)*4);
   %rows = "0";
   for(%i=1;%i<%this.numPoses;%i++)
   {
      %rows = %rows SPC %i*22;
   }
   %this.rows = %rows;

   //Add new controls
   %newTimeH = new GuiTextEditCtrl() {
      profile = "NPCPoseNumberInputProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "0 0";
      extent = "18 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      maxLength = "2";
      validate = "NPCData_Pose_Frame.validateHours("@%this.numPoses-1@");";
      historySize = "0";
      password = "0";
      tabComplete = "0";
      deniedSound = "AudioError";
      sinkAllKeyEvents = "0";
   };
   %newTimeH.setName("NPCData_Pose_"@%this.numPoses-1@"_Time_H");
   %this.add(%newTimeH);

   %newTimeM = new GuiTextEditCtrl() {
      profile = "NPCPoseNumberInputProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "22 0";
      extent = "18 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      maxLength = "2";
      validate = "NPCData_Pose_Frame.validateMinutes("@%this.numPoses-1@");";
      historySize = "0";
      password = "0";
      tabComplete = "0";
      deniedSound = "AudioError";
      sinkAllKeyEvents = "0";
   };
   %newTimeM.setName("NPCData_Pose_"@%this.numPoses-1@"_Time_M");
   %this.add(%newTimeM);

   %this.Pos[%this.numPoses-1] = %newPos = new GuiTextEditCtrl() {
      profile = "NPCPoseTextInputProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "44 0";
      extent = "60 18";//74 18
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      maxLength = "255";
      validate = "NPCData_Pose_Frame.validatePosition("@%this.numPoses-1@");";
      historySize = "0";
      password = "0";
      tabComplete = "1";
      deniedSound = "AudioError";
      sinkAllKeyEvents = "0";
   };
   %newPos.setName("NPCData_Pose_"@%this.numPoses-1@"_Position");
   %this.add(%newPos);

   %this.PosButt[%this.numPoses-1] = %newPosButt = new GuiButtonCtrl() {
      profile = "GuiButtonProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = getWords(vectorAdd(%this.position,vectorAdd(%newPos.position,"61 1")),0,1);
      extent = "12 16";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      command = "NPCData_Section.requestPlayerPosRot(0,"@%this.numPoses-1@");";
      text = "<";
      groupNum = "-1";
      buttonType = "PushButton";
   };
   NPCData_Pose_Scroll.add(%newPosButt);

   %this.Rot[%this.numPoses-1] = %newRot = new GuiTextEditCtrl() {
      profile = "NPCPoseTextInputProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "122 0";
      extent = "74 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      maxLength = "255";
      validate = "NPCData_Pose_Frame.validateRotation("@%this.numPoses-1@");";
      historySize = "0";
      password = "0";
      tabComplete = "1";
      deniedSound = "AudioError";
      sinkAllKeyEvents = "0";
   };
   %newRot.setName("NPCData_Pose_"@%this.numPoses-1@"_Rotation");
   %this.add(%newRot);


   %this.RotButt[%this.numPoses-1] = %newRotButt = new GuiButtonCtrl() {
      profile = "GuiButtonProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = getWords(vectorAdd(%this.position,vectorAdd(%newRot.position,"61 1")),0,1);
      extent = "12 16";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      command = "NPCData_Section.requestPlayerPosRot(1,"@%this.numPoses-1@");";
      text = "<";
      groupNum = "-1";
      buttonType = "PushButton";
   };
   NPCData_Pose_Scroll.add(%newRotButt);

   %newDoAnim = new GuiCheckBoxCtrl() {
      profile = "GuiCheckBoxProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "200 0";
      extent = "13 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      text = "";
      groupNum = "-1";
      buttonType = "ToggleButton";
   };
   %newDoAnim.setName("NPCData_Pose_"@%this.numPoses-1@"_DoAnim");
   %this.add(%newDoAnim);

   %newAnim = new GuiPopUpMenuCtrl() {
      profile = "GuiPopUpMenuProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "217 0";
      extent = "83 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      text = "N/A";
      maxLength = "255";
      maxPopupHeight = "200";
   };
   %newAnim.setName("NPCData_Pose_"@%this.numPoses-1@"_Animation");
   %this.add(%newAnim);

   //Add animation list to animation popup control.
   %data = NPCData_Section;
   for(%i=0;%i<%data.numAnimation;%i++)
   {
      %newAnim.add(%data.animation[%i],%i,0);
   }

   //Adjust Scroll Box and move AddRow button to the bottom.
   //The addRow button won't modify the scroll bar, so we have to do some bullshit with the FrameSet.
   %extent = %this.extent;//Store our current extent.
   %this.extent = getWord(%this.extent,0) SPC getWord(%this.extent,1)+20;//Adjust for the button. (18+2=20)
   %this.setVisible(1);//Force refresh the scroll box while also unhiding the frameset.
   %this.extent = %extent;//Restore our extent.
   NPCData_Pose_AddRow.position = getWord(%this.position,0)+1 SPC getWord(%this.position,1)+getWord(%this.extent,1)+2;//Move the button.
   NPCData_Pose_Scroll.scrollToBottom();//Scroll to the bottom for convenience.
   
   for(%i=0;%i<%this.numPoses;%i++)//I hate framesets so much....
   {
      %this.Pos[%i].extent = "60 18";
      %this.Rot[%i].extent = "60 18";
   }
}
function NPCData_Section::requestPlayerPosRot(%this,%type,%num)//Request player position or rotation for a specific pose number  Timeout 540 ms
{
   cancel(%this.requestNPCDataSched);

   if(%this.currentNPC $= "")
      return;

   if((%timesince = (getSimTime()-%this.requestNPCDataTimeout))<540)
   {
      %timetill = 540 - %timesince;
      NPCEditorGui.addMessage("\c1Requesting player position or rotation in "@%timetill@" ms.");
      %this.requestNPCDataSched = %this.schedule(%timetill,"requestPlayerPosRot",%type,%num);
      return;
   }

   if(!%this.receivingData)
   {
      commandToServer('RequestResetFromNPCOrbit');
      NPCBrick_Camera.currentOrbit = "";
      $mvYawLeftSpeed = "0";

      %this.requestNPCDataTimeout = getSimTime();
      %this.receivingData = true;
      if(!%type)
      {
         NPCEditorGui.addMessage("\c1Requesting position...");
         commandToServer('requestNPCPlayerPos',%this.currentNPC,%num);
      }
      else
      {
         NPCEditorGui.addMessage("\c1Requesting rotation...");
         commandToServer('requestNPCPlayerRot',%this.currentNPC,%num);
      }
   }
}
function NPCData_Pose_Frame::validateHours(%this,%rowNum)
{
   if(isObject(%box = nameToID("NPCData_Pose_"@%rowNum@"_Time_H")))
   {
      %value = %box.getValue();
      %value = %value % 24;
      while(strLen(%value)<2)
      {
         %value = "0"@%value;
      }
      %box.setValue(%value);
   }
}
function NPCData_Pose_Frame::validateMinutes(%this,%rowNum)
{
   if(isObject(%box = nameToID("NPCData_Pose_"@%rowNum@"_Time_M")))
   {
      %value = %box.getValue();
      %value = %value % 60;
      while(strLen(%value)<2)
      {
         %value = "0"@%value;
      }
      %box.setValue(%value);
   }
}
function NPCData_Pose_Frame::validatePosition(%this,%rowNum)
{
   if(isObject(%box = nameToID("NPCData_Pose_"@%rowNum@"_Position")))
   {
      %value = %box.getValue();
      if(getWordCount(%value)>3)//remove extra words
         %value = getWords(%value,0,2);
      while(getWordCount(%value)<3)//add extra words
      {
         %value = %value SPC "0";
      }
      for(%i=0;%i<3;%i++)
      {
         %value = setWord(%value, %i, getWord(%value,%i)*1);//turn text into numbers
      }
      %box.setValue(%value);
   }
}
function NPCData_Pose_Frame::validateRotation(%this,%rowNum)
{
   if(isObject(%box = nameToID("NPCData_Pose_"@%rowNum@"_Rotation")))
   {
      %value = %box.getValue();
      if(getWordCount(%value)>4)//remove extra words
         %value = getWords(%value,0,2);
      while(getWordCount(%value)<3)//add extra words
      {
         %value = %value SPC "0";
      }
      %angle = getWord(%value,3);
      if(%angle<0)
         %neg = true;
      %angle = mAbs(%angle);
      %angle = %angle - mFloor(%angle/$pi)*$pi;//basically the % operation but works with floats.
      if(%neg)
         %angle *= -1;
      %value = vectorNormalize(getWords(%value,0,2)) SPC %angle;//axis SPC angle
      %box.setValue(%value);
   }
}
