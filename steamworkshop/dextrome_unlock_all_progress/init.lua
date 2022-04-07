dofile("mods/dextrome_unlock_all_progress/files/scripts/unlock_spell_cards.lua")

function OnPausedChanged( is_paused, is_inventory_pause ) 
	dofile("mods/dextrome_unlock_all_progress/files/scripts/unlock_enemies.lua")
end