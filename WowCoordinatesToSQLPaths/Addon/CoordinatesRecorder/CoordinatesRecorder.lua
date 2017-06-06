-- Author      : Chucksta
-- Create Date : 9/26/2014 8:13:58 AM-

-- Modified by : Oppahansi
-- Date 	   : 6/6/2017

-- single coordinates will be saved here
Coordinates = {};

-- index to keep track of how many waypoints we have added , needed for adding to the list
CoordinatesIndex = 0;

-- coordinates list
CoordinatesList = {};

-- Slash command to show or hide the addon's interface in-game
SLASH_COORDSMAPPER1 = '/coordsmap';
function SlashCmdList.COORDSMAPPER(msg, editbox)
  if msg == 'show' then
    Frame1:Show();
  else
    Frame1:Hide();    
  end
end

-- register chat message event for system messages
function Frame1_OnLoad()
	this:RegisterEvent("CHAT_MSG_SYSTEM")	
end

-- extract the coordinates from .gps system message
function getCoords(arg1)
	count = 0
	index = 0
	coordinatesAcquired = "false"
	Xcoordinate = ""
	Ycoordinate = ""
	Zcoordinate = ""
	Orientation = ""
	coordinates = ""
	character = string.sub(arg1,1,1) -- grab the 1st char
	
	while character ~= "" and coordinatesAcquired == "false" do
		index = index + 1
		character = string.sub(arg1,index,index)
		
		-- check for X coord
		if character == "X" then
			-- acquire X coordinate
			index = index + 2
			character = string.sub(arg1,index,index)
			while character ~= "Y" do
				Xcoordinate = Xcoordinate .. character
				index = index + 1
				character = string.sub(arg1,index,index)
			end
			-- acquire Y coord
			index = index + 2
			character = string.sub(arg1,index,index)
			while character ~= "Z" do
				Ycoordinate = Ycoordinate .. character
				index = index + 1
				character = string.sub(arg1,index,index)
			end
			-- acquire Z coord
			index = index + 2
			character = string.sub(arg1,index,index)
			while character ~= "O" do
				Zcoordinate = Zcoordinate .. character
				index = index + 1
				character = string.sub(arg1,index,index)
			end
			-- acquire Orientation (removed for now - too many of these was making the animation jerky)
			index = index + 12
			character = string.sub(arg1,index,index)
			while character ~= "g" do
				Orientation = Orientation .. character
				index = index + 1
				character = string.sub(arg1,index,index)
			end
			coordinatesAcquired = "true"
		end
	end

   -- assign new coordinates values
	Coordinates["positionX"] = string.gsub(Xcoordinate, "%s+", "");
	Coordinates["positionY"] = string.gsub(Ycoordinate, "%s+", "");
	Coordinates["positionZ"] = string.gsub(Zcoordinate, "%s+", "");
	Coordinates["orientation"] = string.gsub(Orientation, "%s+", "");

	-- preventing adding an empty waypoint on game start or reloadui
	if (Xcoordinate == "" and Ycoordinate == "" and Zcoordinate == "" and Orientation == "") then
		
	else
	-- add new coordinates to the list
		CoordinatesList[CoordinatesIndex] = Coordinates;
		CoordinatesIndex = CoordinatesIndex + 1;
		Coordinates = {};
	end
end

function Frame1_OnEvent()
	-- react to a system level message (yellow text)
	if event == "CHAT_MSG_SYSTEM" then
		getCoords(arg1)
	else
	end
end

function Button1_OnClick()
    -- trigger system message event
	-- send out GM .gps request (gets continent level coodinates)
	SendChatMessage(".gps", "SAY")
end

function Button2_OnClick()
    -- reset the current waypoint coordinates
	CoordinatesList = {}
	CoordinatesIndex = 0;
	SendChatMessage("Reset done.", "SAY") -- confirm the reset
end