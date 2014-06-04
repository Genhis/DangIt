﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DangIt
{
    /// <summary>
    /// Module that causes failures in the power generator of engines
    /// </summary>
    public class ModuleAlternatorReliability : ModuleBaseFailure
    {
        // The alternator is tied to an engine
        // The engine module also allows to check when the alternator is active
        ModuleEngines engineModule;
        ModuleAlternator alternatorModule;


        public override string DebugName { get { return "DangItAlternator"; } }
        public override string FailureMessage { get { return "Alternator failure!"; } }
        public override string RepairMessage { get { return "Alternator repaired."; } }
        public override string FailGuiName { get { return "Fail alternator"; } }
        public override string EvaRepairGuiName { get { return "Repair alternator"; } }
        public override bool AgeOnlyWhenActive { get { return true; } }


        // The alternator is only active when the engine is actually firing
        public override bool PartIsActive()
        {
            return (this.engineModule.enabled && 
                    this.engineModule.EngineIgnited && 
                   (this.engineModule.currentThrottle > 0));
        }

        public override void DI_OnStart(StartState state)
        {
            if (state == StartState.Editor || state == StartState.None) return;

            this.alternatorModule = part.Modules.OfType<ModuleAlternator>().First();
            this.engineModule = part.Modules.OfType<ModuleEngines>().First();
        }


        public override void DI_Fail()
        {
            this.alternatorModule.enabled = false;
        }


        public override void DI_EvaRepair()
        {
            this.alternatorModule.enabled = true; 
        }

    }
}
