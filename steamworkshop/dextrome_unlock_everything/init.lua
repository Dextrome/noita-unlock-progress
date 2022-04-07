--function OnPausedChanged( is_paused, is_inventory_pause ) 
function OnWorldInitialized() 
	dofile("mods/dextrome_unlock_everything/files/scripts/unlock_enemies.lua")
end

dofile("mods/dextrome_unlock_everything/files/scripts/unlock_spell_cards.lua")
dofile("mods/dextrome_unlock_everything/files/scripts/unlock_spells_used.lua")
dofile("mods/dextrome_unlock_everything/files/scripts/unlock_perks_picked.lua")
dofile("mods/dextrome_unlock_everything/files/scripts/unlock_secrets_and_progress.lua")