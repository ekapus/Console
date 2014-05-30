﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Exceptions;
using Sitecore.Globalization;
using Sitecore.Security.Accounts;

namespace Cognifide.PowerShell.PowerShellIntegrations.Commandlets.Governance
{
    [Cmdlet(VerbsCommon.Unlock, "Item")]
    [OutputType(new[] { typeof(bool) }, ParameterSetName = new[] { "Item from Pipeline", "Item from Path", "Item from ID" })]
    public class UnlockItemCommand : GovernanceUserBaseCommand
    {       

        protected override void ProcessRecord()
        {
            Item sourceItem = GetProcessedRecord();
            ProcessItem(sourceItem);
        }

        private void ProcessItem(Item item)
        {
            LockItem(item);
            if (Recurse)
            {
                foreach (Item child in item.Children)
                {
                    ProcessItem(child);
                }
            }
        }

        private void LockItem(Item item)
        {
            SwitchUser(() =>
            {
                if (item.Locking.IsLocked())
                {
                    if (!item.Access.CanWrite())
                    {
                        if (FailSilently)
                        {
                            WriteObject(false);
                            return;
                        }
                        throw new SecurityException("Cannot modify item '" + item.Name +
                                                    "' because of insufficient privileges.");

                    }
                    WriteObject(item.Locking.Unlock());
                    return;
                }

                WriteObject(true);
            });
        }

    }
}