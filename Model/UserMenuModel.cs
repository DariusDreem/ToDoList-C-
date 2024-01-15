using Framework.Views;
using Framework.Controller;
using Microsoft.VisualBasic.FileIO;
using System;

namespace Framework.Model
{
    public class UserMenuModel
    {
        private static UserMenuModel _instance;

        public static UserMenuModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserMenuModel();
                }
                return _instance;
            }
        }

        public UserMenu MainMenu { get; } = new UserMenu();
        public UserMenuController Controller { get; private set; } = null;
        public string Description { get; set; }
        public PriorityStatus Priority { get; set; } = PriorityStatus.Null;
        public string UserCommand { get; set; }
        public string[] UserCommands { get; set; }
        public string LogDirectory { get; set; }
        public LogWriter Logger = LogWriter.Instance;
        public string Today { get; set; }
        public bool AutoRead { get; set; } = false;

        // Constructeur privé pour empêcher l'instanciation directe
        private UserMenuModel()
        {
            Controller = new UserMenuController(this);
            Today = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
