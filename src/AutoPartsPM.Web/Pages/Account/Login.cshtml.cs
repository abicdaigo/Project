using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;

namespace AutoPartsPM.Web.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "ユーザー名は必須です")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "パスワードは必須です")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    private static readonly string[] AppRoles = { "PM_Admins", "PM_Managers", "PM_Members" };

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var domain = _configuration["Authentication:Domain"] ?? string.Empty;

        try
        {
            // ユーザー名からドメインプレフィックスを分離
            var userName = Input.UserName;
            if (userName.Contains('\\'))
            {
                var parts = userName.Split('\\', 2);
                domain = string.IsNullOrEmpty(domain) ? parts[0] : domain;
                userName = parts[1];
            }

            using var context = new PrincipalContext(
                ContextType.Domain,
                string.IsNullOrEmpty(domain) ? null : domain);

            if (!context.ValidateCredentials(userName, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "ユーザー名またはパスワードが正しくありません");
                return Page();
            }

            // AD グループ取得
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, string.IsNullOrEmpty(domain)
                    ? userName
                    : $"{domain}\\{userName}"),
                new(ClaimTypes.AuthenticationMethod, "Password"),
            };

            using var userPrincipal = UserPrincipal.FindByIdentity(context, userName);
            if (userPrincipal is not null)
            {
                var groups = userPrincipal.GetGroups()
                    .Select(g => g.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var role in AppRoles)
                {
                    if (groups.Contains(role))
                        claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            if (!claims.Any(c => c.Type == ClaimTypes.Role))
            {
                ModelState.AddModelError(string.Empty, "このシステムへのアクセス権がありません。IT管理者にお問い合わせください。");
                return Page();
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            _logger.LogInformation("ユーザー {UserName} がフォームログインしました", claims[0].Value);

            return LocalRedirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl! : Url.Content("~/"));
        }
        catch (PrincipalServerDownException)
        {
            _logger.LogError("AD サーバーに接続できません (Domain: {Domain})", domain);
            ModelState.AddModelError(string.Empty, "認証サーバーに接続できません。ネットワーク接続を確認してください。");
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ログイン処理中にエラーが発生しました");
            ModelState.AddModelError(string.Empty, "ログイン処理中にエラーが発生しました");
            return Page();
        }
    }
}
