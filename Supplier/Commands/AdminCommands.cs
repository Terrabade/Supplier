﻿using CSF;
using CSF.TShock;
using Supplier.Extensions;

namespace Supplier
{
    [RequirePermission("tbc.admin")]
    public class AdminCommands : TSModuleBase<TSCommandContext>
    {
        #region /infchest <subcmd> - manage infinite chests - tbc.admin
        [Command("infchest")]
        [Description("Command used to manage infinite chest.")]
        public IResult InfChest(string sub = "")
        {
            var plrState = Context.Player.GetPlayerOperationState();

            // inform player if command is being used while a state is already set
            if (plrState.InfChestAdd || plrState.InfChestAddBulk || plrState.InfChestDelete && sub != "help")
            {
                Info("If an existing chest selection operation is currently unfulfilled, it will be overriden with the new request.");
            }

            // switch for sub commands
            switch (sub)
            {
                // if /infchest add was executed
                case "add":
                    {
                        plrState.InfChestAdd = true;
                        return Success("Open a chest to make it infinite. Type /cancel to cancel.");
                    }
                // if /infchest addbulk was executed
                case "addbulk":
                    {
                        plrState.InfChestAddBulk = true;
                        return Success("Open chests to make them infinite, type /cancel to stop.");
                    }
                // if /infchest del was executed
                case "del":
                    {
                        plrState.InfChestDelete = true;
                        return Success("Open a chest to delete it from Supplier. Type /cancel to cancel.");
                    }
                // if no sub command was executed, if it was invalid, or if they entered /infchest help
                case "help":
                default:
                    {
                        Info("Help commands for /infchest:");
                        Info("/infchest add - allows the user to create an infinite chest");
                        Info("/infchest addbulk - allows the user to continuously create infinite chests until /cancel is used");
                        Info("/infchest del - deletes an infinite chest");
                        return Info("/infchest help - shows this help message");
                    }
            }
        }
        #endregion

        #region /cancel - cancels infinite chest selection - tbc.admin
        [Command("cancel")]
        [Description("Cancels infinite chest actions.")]
        public IResult InfChest()
        {
            // set all data to false and tell player if a selection operation operation was enabled
            var plrState = Context.Player.GetPlayerOperationState();
            if (plrState.InfChestAdd || plrState.InfChestAddBulk || plrState.InfChestDelete)
            {
                plrState.SetAllOperationStatesFalse();
                return Success("Cancelled chest operation.");

            }
            // tell the player they weren't selecting anything
            return Error("You weren't selecting anything, so nothing was cancelled.");

        }
        #endregion
    }
}
