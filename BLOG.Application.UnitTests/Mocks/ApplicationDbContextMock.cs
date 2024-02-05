using BLOG.Application.Common.Abstractions;
using BLOG.Infrastructure.Persistance;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Common;

namespace BLOG.Application.UnitTests.Mocks
{
    public class ApplicationDbContextMock : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection).Options;
        }

        public ApplicationDbContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();

                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureCreated();

                    context.Users.AddRange(GetUserDbSet());
                    context.Posts.AddRange(GetPostDbSet());
                    context.Comments.AddRange(GetCommentDbSet());

                    context.SaveChanges();
                }
            }

            return new ApplicationDbContext(CreateOptions());
        }

        public ApplicationDbContext CreateContext(Lazy<ICurentUserService> userService)
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();

                using (var context = new ApplicationDbContext(options, userService))
                {
                    context.Database.EnsureCreated();

                    context.Users.AddRange(GetUserDbSet());
                    context.Posts.AddRange(GetPostDbSet());
                    context.Comments.AddRange(GetCommentDbSet());

                    context.SaveChanges();
                }
            }

            return new ApplicationDbContext(CreateOptions(), userService);
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        //private readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        //public ApplicationDbContextMock()
        //{
        //    _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "BLOG")
        //        .Options;

        //    using var context = new ApplicationDbContext(_contextOptions);

        //    context.Users.AddRange(GetUserDbSet());
        //    context.Posts.AddRange(GetPostDbSet());
        //    context.Comments.AddRange(GetCommentDbSet());

        //    context.SaveChanges();
        //}

        //public ApplicationDbContext CreateContext() => new ApplicationDbContext(_contextOptions);

        private List<Domain.Model.ApplicationUser.ApplicationUser> GetUserDbSet()
        {
            var data = new List<Domain.Model.ApplicationUser.ApplicationUser>
            {
                new Domain.Model.ApplicationUser.ApplicationUser
                {
                    Id = "9baf84a0-f581-4606-afda-4537a8da5a7d",
                    UserName = "admin@test.com",
                    NormalizedUserName = "ADMIN@TEST.COM",
                    Email = "admin@test.com",
                    NormalizedEmail = "ADMIN@TEST.COM",
                    NickName = "Admin"
                },
                new Domain.Model.ApplicationUser.ApplicationUser
                {
                    Id = "48a1f000-7047-41ce-9b63-c9ba458487d5",
                    UserName = "user@example.com",
                    NormalizedUserName = "USER@EXAMPLE.COM",
                    Email = "user@example.com",
                    NormalizedEmail = "USER@EXAMPLE.COM",
                    NickName = "Dawid"
                },
                new Domain.Model.ApplicationUser.ApplicationUser
                {
                    Id = "b1bcfaf2-3164-4f11-a179-a15990a3c92f",
                    UserName = "user2@example.com",
                    NormalizedUserName = "USER3@EXAMPLE.COM",
                    Email = "user2@example.com",
                    NormalizedEmail = "USER3@EXAMPLE.COM",
                    NickName = "dawid002"
                },
                new Domain.Model.ApplicationUser.ApplicationUser
                {
                    Id = "0905d455-2eb2-46ba-b596-7ecad6b61fec",
                    UserName = "user3@example.com",
                    NormalizedUserName = "USER3@EXAMPLE.COM",
                    Email = "user3@example.com",
                    NormalizedEmail = "USER3@EXAMPLE.COM",
                    NickName = "sadasd"
                }
            };

            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var json = File.ReadAllText(path + "/DbSet/users.json");

            data = JsonConvert.DeserializeObject<List<Domain.Model.ApplicationUser.ApplicationUser>>(json);

            return data;
        }

        private List<Domain.Model.Post.Post> GetPostDbSet()
        {
            var data = new List<Domain.Model.Post.Post>
            {
                new Domain.Model.Post.Post
                {
                    Id = 1,
                    Title = "Wykluczenie Węgier z NATO? Gen. Polko: \"Orban jawnie wspiera terrorystę Putina\"",
                    Description = "Władimir Putin chciałby doprowadzić do przerwy w działaniach wojennych - uważa gen. Roman Polko w rozmowie z portalem Wprost. I wskazuje w jakim celu miałoby się to stać.",
                    Content = "<p><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Gen. Roman Polko w&nbsp;rozmowie z&nbsp;Wprost podsumowuje blisko dwa lata wojny w&nbsp;Ukrainie. Przekonuje, że obietnice Europy i&nbsp;Stanów Zjednoczonych w&nbsp;zakresie pomocy Ukrainie \"zostały zrealizowane najwyżej w&nbsp;50 procentach\". -&nbsp;Ofensywa ukraińska nie przebiegła tak, jak się spodziewaliśmy, bo </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">trudno w&nbsp;obliczu braków w&nbsp;uzbrojeniu realizować działania ofensywne</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">. Na dodatek Ukraina boryka się z&nbsp;problemami wewnętrznymi -&nbsp;mówi Polko.</span></span></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Były dowódca elitarnej jednostki GROM przekonuje, że </span></span><a href=\"https://wiadomosci.wp.pl/rosja-w-2024-roku-konsolidacja-mimo-wojny-6981449846721024a?utm_source=msn&amp;utm_medium=agregator\" target=\"_blank\"><strong><span style=\"color:var(--accent-foreground-rest);\"><span style=\"background-color:rgb(59, 59, 59);\">Władimir Putin</span></span></strong></a><strong><span style=\"background-color:rgb(59, 59, 59);\"> widząc problemy Ukrainy chciałby doprowadzić do przerwy w&nbsp;działaniach wojennych</span></strong><span style=\"background-color:rgb(59, 59, 59);\">.</span></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">- Pod pozorem rozejmu Putin chciałby odbudować swój potencjał bojowy. Gdy to zrobi, to </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">uderzy ponownie, bo jego celem nie jest Ukraina tylko nowy porządek geopolityczny</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\"> i&nbsp;podporządkowanie sobie Europy Środkowo-Wschodniej -&nbsp;przekonuje w&nbsp;rozmowie z&nbsp;\"Wprost\" gen. Roman Polko.</span></span></p><p style=\"text-align:start\"><em><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Dalsza część artykułu pod materiałem wideo</span></span></em></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Polko dostrzega brak jedności w&nbsp;Europie ale uważa, że Zachód nie powinien się zatrzymywać w&nbsp;swojej pomocy.</span></span></p><p style=\"text-align:center\"><em><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">- Blokowanie przez konia trojańskiego czyli Wiktora Orbana pomocy dla Ukrainy jest skandaliczne. Nie miałbym nic przeciwko wykluczeniu Węgrów z&nbsp;NATO, bo Orban jawnie wspiera terrorystę Putina -&nbsp;uważa Roman Polko.</span></span></em></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Gen. Polko uważa, że Ukraina powinna dostawać więcej sprzętu wojskowego, zwłaszcza nowego, bo \"to jest nasza wojna, którą toczą Ukraińcy\". Tak się jednak nie dzieje, bo działania Zachodu są za bardzo propagandowe, a&nbsp;za mało rzeczywiste.</span></span></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">- </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Putin przed wojną zbudował swoją piątą kolumnę </span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">-&nbsp;miał kontakty, relacje, korumpował niektórych polityków. Na szczęście to nie zaprocentowało tak, jak sobie wymyślił, ale jest dużo do zrobienia, żeby to uporządkować -&nbsp;uważa gen. Roman Polko.</span></span></p>",
                    UserId = "48a1f000-7047-41ce-9b63-c9ba458487d5",
                    PublishedAt = DateTime.Parse("2024-01-06 18:29:02.5874955"),
                    Image = "9901d690-59e0-4991-a460-b0a388ce5a20.jpg"
                },
                new Domain.Model.Post.Post
                {
                    Id = 2,
                    Title = "Już wszystko jasne. To z nimi Biało-Czerwoni zagrają w finale turnieju United Cup!",
                    Description = "Tenisowa reprezentacja Niemiec wygrała po zaciętej rywalizacji z Australią 2:1 w półfinale turnieju drużyn mieszanych United Cup i będzie rywalem Biało-Czerwonych w zaplanowanym na niedzielę finale imprezy. Decydujące spotkanie odbędzie się w Sydney.",
                    Content = "<p><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Wcześniej awans do finału wywalczyła reprezentacja Polski, która wygrała 3:0 z Francją. Swoje spotkania wygrali </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Iga Świątek</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">, </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Hubert Hurkacz</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">, a także duet </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Katarzyna Kawa</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\"> i </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Jan Zieliński</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">.</span></span></p><h2 style=\"text-align:start\"><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Zacięty mecz Niemiec z Australią</span></span></strong></h2><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Australijczycy i Niemcy stoczyli zacięte spotkanie w walce o finał turnieju United Cup. </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Angelique Kerber</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\"> co prawda dała naszym zachodnim sąsiadom prowadzenie, dzięki zwycięstwu z </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Ajlą Tomljanović</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">, w drugim pojedynku </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Alexander Zverev</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\"> przegrał jednak z </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Alexem de Minaurem</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\"> i finałowego rywala Biało-Czerwonych musiała wyłonić rywalizacja mikstowa.</span></span></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">W decydującym meczu wystąpili </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Storm Hunter</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\"> i </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Matthew Ebden</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\"> po stronie gospodarzy turnieju, a także Zverev i </span></span><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Laura Siegemund</span></span></strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">. Potyczka była niezwykle zacięta, dwa pierwsze sety kończyły się tie-breakami (pierwszy padł łupem Niemców, drugi Australijczyków), a finalistę United Cup wyłonić musiał super tie-break.</span></span></p><h2 style=\"text-align:start\"><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Kiedy finał turnieju United Cup?</span></span></strong></h2><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Ostatecznie do finału awans wywalczyli Niemcy, którzy potrzebowali jednak aż pięć piłek meczowych, by zakończyć potyczkę (w między czasie dwie okazje na zamknięcie rywalizacji mieli ich przeciwnicy). Ostatnie spotkanie turnieju United Cup zaplanowano na niedzielę, 7 stycznia. Początek o 7.30 czasu polskiego.</span></span></p><p style=\"text-align:start\"><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Półfinał turnieju United Cup</span></span></strong></p><p style=\"text-align:start\"><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Niemcy - Australia 2:1</span></span></strong></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Angelique Kerber - Ajla Tomljanović 4:6, 6:2, 7:6</span></span></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Alexander Zverev - Alex de Minaur 7:5, 3:6, 4:6</span></span></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(59, 59, 59);\">Alexander Zverev/Laura Siegemund - Storm Hunter/Matthew Ebden 7:6, 6:7, 15:13</span></span></p>",
                    UserId = "b1bcfaf2-3164-4f11-a179-a15990a3c92f",
                    PublishedAt = DateTime.Parse("2024-01-06 18:37:30.1039402"),
                    Image = "ab68c7df-d368-4b6f-9fb9-d78d8728edf3.jpg"
                },
                new Domain.Model.Post.Post
                {
                    Id = 3,
                    Title = "Sensacja goni sensację. Wszyscy patrzyli, co zrobi Sabalenka",
                    Description = "Aryna Sabalenka nie trwoni sił w Australian Open 2024. Wiceliderka rankingu w nieco ponad godzinę rozprawiła się z Czeszką Brendą Fruhvirtovą. Anastazja Zacharowa będzie kolejną rywalką Magdaleny Fręch.",
                    Content = "<p><a href=\"https://sportowefakty.wp.pl/tenis/aryna-sabalenka\" target=\"_blank\"><span style=\"color:var(--accent-foreground-rest);\"><span style=\"background-color:rgb(36, 36, 36);\">Aryna Sabalenka</span></span></a><span style=\"background-color:rgb(36, 36, 36);\">&nbsp;(WTA 2) występ w Australian Open rozpoczęła od rozbicia 6:0, 6:1 Niemki </span><a href=\"https://sportowefakty.wp.pl/tenis/ella-seidel\" target=\"_blank\"><span style=\"color:var(--accent-foreground-rest);\"><span style=\"background-color:rgb(36, 36, 36);\">Elli Seidel</span></span></a><span style=\"background-color:rgb(36, 36, 36);\">. Kolejnej rywalce oddała pięć gemów. Wiceliderka rankingu pokonała 6:3, 6:2 </span><a href=\"https://sportowefakty.wp.pl/tenis/brenda-fruhvirtova\" target=\"_blank\"><span style=\"color:var(--accent-foreground-rest);\"><span style=\"background-color:rgb(36, 36, 36);\">Brendę Fruhvirtovą</span></span></a><span style=\"background-color:rgb(36, 36, 36);\">&nbsp;(WTA 107).</span></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(36, 36, 36);\">W pierwszym secie Fruhvirtova z 0:2 wyrównała na 2:2. Później Sabalenka wiodła prym i zdobyła 10 z 13 kolejnych gemów. W ciągu 67 minut Białorusinka zgarnęła 21 z 26 punktów przy swoim pierwszym podaniu oraz 10 z 12 przy siatce. Posłała 30 kończących uderzeń przy 22 niewymuszonych błędach. Czeszka miała siedem piłek wygranych bezpośrednio przy 17 pomyłkach.</span></span></p><p><span style=\"color:inherit;\">&nbsp; Kontynuuj czytanie</span></p><p></p><p style=\"text-align:start\"><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(36, 36, 36);\">Sabalenka do rywalizacji przystąpiła jako obrończyni tytułu. Rok temu w Melbourne wywalczyła swój pierwszy wielkoszlemowy tytuł w singlu. Białorusinka w każdej z czterech wielkoszlemowych imprez grała co najmniej w półfinału. Jego drugi najlepszy rezultat to finał US Open 2023. Jej kolejną rywalką będzie </span></span><a href=\"https://sportowefakty.wp.pl/tenis/lesia-curenko\" target=\"_blank\"><span style=\"color:var(--accent-foreground-rest);\"><span style=\"background-color:rgb(36, 36, 36);\">Łesia Curenko</span></span></a><span style=\"background-color:rgb(36, 36, 36);\">&nbsp;lub </span><a href=\"https://sportowefakty.wp.pl/tenis/rebeka-masarova\" target=\"_blank\"><span style=\"color:var(--accent-foreground-rest);\"><span style=\"background-color:rgb(36, 36, 36);\">Rebeka Masarova</span></span></a><span style=\"background-color:rgb(36, 36, 36);\">.</span></p><p style=\"text-align:start\"><strong><span style=\"color:rgb(255, 255, 255);\"><span style=\"background-color:rgb(36, 36, 36);\">ZOBACZ WIDEO: Zobacz, gdzie Milik zabrał ukochaną. Jej zdjęcia zachwycają</span></span></strong></p>",
                    UserId = "9baf84a0-f581-4606-afda-4537a8da5a7d",
                    PublishedAt = DateTime.Parse("2024-01-17 13:59:44.8916230"),
                    Image = "d31668fb-4a7b-4240-8a5a-6b0a3816b7af.jpg"
                },
            };

            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var json = File.ReadAllText(path + "/DbSet/posts.json");

            data = JsonConvert.DeserializeObject<List<Domain.Model.Post.Post>>(json);

            return data;
        }

        private List<Domain.Model.Comment.Comment> GetCommentDbSet()
        {
            var data = new List<Domain.Model.Comment.Comment>
            {
                new Domain.Model.Comment.Comment
                {
                    Id = 1,
                    Content = "Super",
                    PublishedAt = DateTime.Parse("2024-01-10 00:40:18.9036758"),
                    PostId = 1,
                    UserId = "48a1f000-7047-41ce-9b63-c9ba458487d5"
                },
                new Domain.Model.Comment.Comment
                {
                    Id = 2,
                    Content = "Wow",
                    PublishedAt = DateTime.Parse("2024-01-17 01:01:13.4173023"),
                    PostId = 1,
                    UserId = "0905d455-2eb2-46ba-b596-7ecad6b61fec"
                },
                new Domain.Model.Comment.Comment
                {
                    Id = 3,
                    Content = "No nieźle",
                    PublishedAt = DateTime.Parse("2024-01-17 01:03:52.6805877"),
                    PostId = 2,
                    UserId = "48a1f000-7047-41ce-9b63-c9ba458487d5"
                },
                new Domain.Model.Comment.Comment
                {
                    Id = 4,
                    Content = "Tylko na to czekałem",
                    PublishedAt = DateTime.Parse("2024-01-17 01:03:52.6805877"),
                    PostId = 2,
                    UserId = "0905d455-2eb2-46ba-b596-7ecad6b61fec"
                },
                new Domain.Model.Comment.Comment
                {
                    Id = 5,
                    Content = "Kozak",
                    PublishedAt = DateTime.Parse("2024-01-18 00:09:30.9208814"),
                    PostId = 3,
                    UserId = "48a1f000-7047-41ce-9b63-c9ba458487d5"
                },
                new Domain.Model.Comment.Comment
                {
                    Id = 6,
                    Content = "Takie tam",
                    PublishedAt = DateTime.Parse("2024-01-18 00:26:13.9124926"),
                    PostId = 3,
                    UserId = "0905d455-2eb2-46ba-b596-7ecad6b61fec"
                }
            };

            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var json = File.ReadAllText(path + "/DbSet/comments.json");

            data = JsonConvert.DeserializeObject<List<Domain.Model.Comment.Comment>>(json);

            return data;
        }
    }
}
