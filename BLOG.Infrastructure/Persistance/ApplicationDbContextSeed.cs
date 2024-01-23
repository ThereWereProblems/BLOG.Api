using AutoMapper;
using Azure.Core;
using BLOG.Application.Common.Abstractions;
using BLOG.Application.Result;
using BLOG.Domain.DTO;
using BLOG.Domain.Model.ApplicationUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOG.Infrastructure.Persistance
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(WebApplication app)
        {
            try
            {
                var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (context.Database.CanConnect())
                {
                    // dodanie roli do bazy danych
                    var json = File.ReadAllText("../BLOG.Domain/SeedData/roles.json");
                    JArray roles = JArray.Parse(json);

                    if (roles != null && roles.Any())
                    {
                        foreach (JObject role in roles)
                        {
                            if (!await roleManager.RoleExistsAsync(role.GetValue("Name").ToString()))
                                await roleManager.CreateAsync(new IdentityRole(role.GetValue("Name").ToString()));
                        }
                    }


                    // dodanie użytkowników do bazy danych
                    json = File.ReadAllText("../BLOG.Domain/SeedData/users.json");
                    JArray users = JArray.Parse(json);

                    if (users != null && users.Any())
                    {
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
                        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                        foreach (JObject obj in users)
                        {
                            if (await userManager.FindByEmailAsync(obj.GetValue("Email").ToString()) == null)
                            {
                                var user = obj.ToObject<RegisterAppUserDTO>();
                                var appUser = mapper.Map<ApplicationUser>(user);

                                var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
                                await userStore.SetUserNameAsync(appUser, user.Email, CancellationToken.None);
                                await emailStore.SetEmailAsync(appUser, user.Email, CancellationToken.None);
                                var result = await userManager.CreateAsync(appUser, user.Password);

                                // dodanie roli
                                if(result.Succeeded)
                                {
                                    await userManager.AddToRolesAsync(appUser, obj.GetValue("Roles").ToObject<List<string>>());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
