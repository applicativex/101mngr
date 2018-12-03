using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace _101mngr.WebApp.Controllers
{
    public static class ControllerExt
    {
        public static long GetSubjectId(this Controller controller)
        {
            var claim = controller.HttpContext.User.FindFirstValue("sub");

            if (claim == null) throw new InvalidOperationException("sub claim is missing");
            return long.Parse(claim);
        }
    }
}