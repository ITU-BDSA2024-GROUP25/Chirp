using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
// Using "Microsoft.AspNetCore.Identity;" instead of "Microsoft.AspNetCore.Identity.EntityFrameworkCore"
// Due to using a newer version of .net asp Core compared to the book: ASP.NET Core in Action

namespace Chirp.Core;

/// <summary>
/// This is the entity model that represents a user and is used for claims / authentication
/// </summary>
public class AppUser : IdentityUser { }