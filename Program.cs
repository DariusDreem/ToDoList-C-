using Framework;
using Framework.Controller;
using Framework.Model;
using Microsoft.EntityFrameworkCore;

class Runner
{
    public static void Main(string[] args)
    {
        UserMenuModel menu = UserMenuModel.Instance;
        menu.MainMenu.Start();
        menu.Controller.StartMenu();
    }
}