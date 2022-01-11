using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGestures.Common.Config.Impl {
    public class ConfigData {
        public   bool AutoStart { get; set; } 
        public  bool AutoCheckForUpdate { get; set; }

        public  int PathTrackerTriggerButton { get; set; }
        public  int PathTrackerInitialValidMove { get; set; }
        public  bool PathTrackerStayTimeout { get; set; }
        public  int PathTrackerStayTimeoutMillis { get; set; }
        public  bool PathTrackerInitialStayTimeout { get; set; }
        public  int PathTrackerInitialStayTimoutMillis { get; set; }
        public  bool PathTrackerPreferCursorWindow { get; set; }

        //历史原因， 字符串不变
        public  bool PathTrackerDisableInFullScreen { get; set; }

        public  bool GestureViewShowPath { get; set; }
        public  bool GestureViewShowCommandName { get; set; }
        public  bool GestureViewFadeOut { get; set; }
        public  int GestureViewMainPathColor { get; set; }
        public  int GestureViewMiddleBtnMainColor { get; set; }
        public  int GestureViewAlternativePathColor { get; set; }
        public  int GestureViewXBtnPathColor { get; set; }


        public  bool IsFirstRun { get; set; }
        public  string Is360EverDected { get; set; }

        public  bool TrayIconVisible { get; set; }

        public  bool GestureParserEnableHotCorners { get; set; }
        public  bool GestureParserEnable8DirGesture { get; set; }

        public  bool GestureParserEnableRubEdges { get; set; }
        public  string PauseResumeHotKey { get; set; }
        public  bool EnableWindowsKeyGesturing { get; set; }
        public string FileVersion { get; set; }
    }
}
