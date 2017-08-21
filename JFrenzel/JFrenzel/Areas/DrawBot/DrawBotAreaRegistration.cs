using System.Web.Mvc;

namespace JFrenzel.Areas.DrawBot
{
    public class DrawBotAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DrawBot";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DrawBot_default",
                "DrawBot/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}