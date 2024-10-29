using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
// had to use "Microsoft.AspNetCore.Identity;" instead of "Microsoft.AspNetCore.Identity.EntityFrameworkCore"
// this appears to be due to me using a newer version of .net asp Core compared to the book
namespace Chirp.Core;

public class AppUser : IdentityUser { }