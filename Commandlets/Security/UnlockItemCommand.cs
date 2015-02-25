﻿using System.Management.Automation;
using Cognifide.PowerShell.Core.Extensions;
using Cognifide.PowerShell.Core.Utility;
using Sitecore.Data.Items;

namespace Cognifide.PowerShell.Commandlets.Security
{
    [Cmdlet(VerbsCommon.Unlock, "Item", SupportsShouldProcess = true)]
    [OutputType(typeof (bool), ParameterSetName = new[] {"Item from Pipeline", "Item from Path", "Item from ID"})]
    public class UnlockItemCommand : BaseEditItemCommand
    {
        protected override void EditItem(Item item)
        {
            if (ShouldProcess(item.GetProviderPath(), "Unlock Item"))
            {
                item.Locking.Unlock();
            }
        }
    }
}