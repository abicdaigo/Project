using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace AutoPartsPM.Web.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private static readonly string[] AppRoles = { "PM_Admins", "PM_Managers", "PM_Members" };

    /// <summary>
    /// Windows 認証でサインインし、Cookie を発行するエンドポイント。
    /// JS fetch で呼び出され、成功時は 200 + リダイレクト先 URL を返す。
    /// </summary>
    [HttpGet("windows-signin")]
    [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
    public async Task<IActionResult> WindowsSignIn(
        [FromQuery] string? returnUrl,
        [FromQuery] bool ajax = false)
    {
        var windowsIdentity = User.Identity as WindowsIdentity
            ?? throw new InvalidOperationException("Windows identity not available");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, windowsIdentity.Name),
            new(ClaimTypes.AuthenticationMethod, "Windows"),
        };

        // Windows トークンからアプリロールへマッピング
        foreach (var role in AppRoles)
        {
            if (User.IsInRole(role))
                claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

        var destination = Url.IsLocalUrl(returnUrl) ? returnUrl! : Url.Content("~/");

        // ajax=true の場合は JS にリダイレクト先を文字列で返す
        if (ajax)
            return Content(destination);

        return LocalRedirect(destination);
    }
}
