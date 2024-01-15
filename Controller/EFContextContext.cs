using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Model;
using Framework.Views;

namespace Framework.Controller
{
    public class EFContextController
    {
        private readonly EFContextModel db;

        public EFContextController (EFContextModel dbContext)
        {
            db = dbContext;
        }
    }
}
