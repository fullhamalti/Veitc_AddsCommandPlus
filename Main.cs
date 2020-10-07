// Copyright © 2020 Fullham Alfayet
// Licensed under terms of the GPL Version 3. See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Text;

using Sims3.SimIFace;

namespace Veitc.AddsCommandPlus
{
    public class _Main
    {
        [Tunable]
        public static bool kMainl = false;
        public static bool IsMLoaded = false;
        private static bool DataBool01 = false;

        protected internal static int _main()
        {
            IsMLoaded = true;
            try
            {
                Comman.FuncTask.InitClass();
                VCommands.InitClass();
                World.OnWorldLoadFinishedEventHandler += OnWorldLoadFinished;
                World.OnWorldQuitEventHandler += OnWorldQuit;
                World.OnStartupAppEventHandler += World_OnStartupAppEventHandler;
            }
            catch (Exception ex)
            {
                Comman.PrintException("_main(): failed", ex);
                return 1;
            }

            return 0;
        }

        static _Main() { OnLoadingMod(); _main(); }

        public static string vGetTextVersion()
        {
            return _thisAssembly._version;
        }

        public static string vGetTextLicense()
        {
            return _thisAssembly._license;
        }

        private static void OnLoadingMod()
        {
            if (DataBool01)
                return;
            DataBool01 = true;
            DEBUG_Utils.Print("Called OnLoadingMod()");
        }

        private static void OnWorldQuit(object sender, EventArgs e)
        {
            DEBUG_Utils.Print("Called OnWorldQuit() sender: " + (sender != null ? sender.GetType().ToString() : "NULL"));
        }

        private static void OnWorldLoadFinished(object sender, EventArgs e)
        {
            DEBUG_Utils.Print("Called OnWorldLoadFinished() sender: " + (sender != null ? sender.GetType().ToString() : "NULL"));
            Comman.OnWorldLoadFinished();
        }

        private static void World_OnStartupAppEventHandler(object sender, EventArgs e)
        {
            if (CommandSystem.Exists())
            {
                CommandHandler d = delegate(object[] parameters) {
                    object o = null;
                    Comman.FuncTask.CreateTask(delegate
                    {
                        if (o == null)
                            VCommands._RunCommands(parameters);
                    });
                    return 0; 
                };
                CommandSystem.RegisterCommand("veitc", "Using veitc [...]", d, false);
            }
            else
                Comman.PrintMessage("Command System don't have Exists!!\nCan't Run Register Command", false, float.MaxValue);
        }
    }
}
