using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_Programming_Project.Models;

namespace Web_Programming_Project.Controllers
{
    // Sadece giriş yapmış kullanıcılar bu Controller'a erişebilir
    [Authorize]
    public class EventsController : Controller
    {
        private DbPersonal db = new DbPersonal();

        // ------------------- LİSTELEME (INDEX) -------------------
        // GET: Events
        public ActionResult Index()
        {
            // 1. Admin Kontrolü
            string currentEmail = User.Identity.Name;
            var currentUser = db.Users.FirstOrDefault(x => x.Email == currentEmail);

            if (currentUser != null && currentUser.Role == "Admin")
            {
                // Etkinlikleri tarihe göre sıralı getir (En yakın en üstte)
                var events = db.Events.OrderBy(e => e.Date).ToList();
                return View(events);
            }
            else
            {
                // Admin değilse yetkisiz işlem, Ana Sayfaya yolla
                return RedirectToAction("Index", "Home");
            }
        }

        // ------------------- DETAYLAR (DETAILS) -------------------
        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Event @event = db.Events.Find(id);
            if (@event == null) return HttpNotFound();
            return View(@event);
        }

        public ActionResult Details_User(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Event @event = db.Events.Find(id);
            if (@event == null) return HttpNotFound();
            return View(@event);
        }

        // ------------------- OLUŞTURMA (CREATE) -------------------
        // GET: Events/Create
        // GET: Events/Create
        // GET: Events/Create
        // GET: Events/Create
        public ActionResult Create()
        {
            // 1. Admin Kontrolü (Senin mevcut kodun)
            string currentEmail = User.Identity.Name;
            var currentUser = db.Users.FirstOrDefault(x => x.Email == currentEmail);
            if (currentUser == null || currentUser.Role != "Admin") return RedirectToAction("Index", "Home");

            // --- A. KATEGORİ LİSTESİ ---
            List<SelectListItem> categories = new List<SelectListItem>
    {
        new SelectListItem { Text = "Konser", Value = "Konser" },
        new SelectListItem { Text = "Sinema", Value = "Sinema" },
        new SelectListItem { Text = "Tiyatro", Value = "Tiyatro" },
        new SelectListItem { Text = "Spor", Value = "Spor" },
        new SelectListItem { Text = "Festival", Value = "Festival" }
    };
            ViewBag.CategoryList = categories;

            // --- B. ŞEHİR LİSTESİ (81 İL) ---
            // Uzun bir liste olduğu için dizi tanımlayıp SelectListItem'a çeviriyoruz
            string[] citiesArray = new string[] {
        "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Isparta", "Mersin", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce"
    };

            List<SelectListItem> cityList = new List<SelectListItem>();
            foreach (var city in citiesArray)
            {
                cityList.Add(new SelectListItem { Text = city, Value = city });
            }
            ViewBag.CityList = cityList;

            // --- C. RESİM LİSTESİ (Zaten yapmıştık, burası aynen kalıyor) ---
            string folderPath = Server.MapPath("~/event_photos/");
            List<SelectListItem> imageFiles = new List<SelectListItem>();
            imageFiles.Add(new SelectListItem { Text = "- Bir Resim Seçiniz -", Value = "" });

            if (System.IO.Directory.Exists(folderPath))
            {
                string[] allFiles = System.IO.Directory.GetFiles(folderPath);
                foreach (string file in allFiles)
                {
                    string extension = System.IO.Path.GetExtension(file).ToLower();
                    string fileName = System.IO.Path.GetFileName(file);
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".webp")
                    {
                        imageFiles.Add(new SelectListItem { Text = fileName, Value = "/event_photos/" + fileName });
                    }
                }
            }
            else
            {
                imageFiles.Add(new SelectListItem { Text = "Klasör Bulunamadı", Value = "" });
            }
            ViewBag.ImageFileList = imageFiles;
            // -------------------------------------------------------------

            return View();
        }

        // ------------------- OLUŞTURMA (CREATE - POST) -------------------
        // ------------------- OLUŞTURMA (CREATE - POST) -------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        // DÜZELTME 1: Bind içinden "Price" kaldırıldı. Biz manuel atayacağız.
        public ActionResult Create([Bind(Include = "EventId,Title,Artist,Description,Category,ImageUrl,Date,City,Venue,Capacity,DiscountRate,SubCategory")] Event @event, decimal? SinglePrice, string[] ZoneNames, decimal[] ZonePrices)
        {
            // 1. FİYAT ATAMA MANTIĞI
            if (@event.Category == "Sinema")
            {
                // Sinema ise tek fiyat
                @event.Price = SinglePrice ?? 0;
            }
            else
            {
                // Tiyatro/Konser ise en düşük bölge fiyatı vitrin fiyatı olur
                if (ZonePrices != null && ZonePrices.Length > 0)
                {
                    @event.Price = ZonePrices.Min();
                }
                else
                {
                    @event.Price = 0;
                }
            }

            // 2. ModelState Temizliği
            // Price'ı formdan almadığımız için ModelState hata verebilir, bunu temizliyoruz.
            if (ModelState.ContainsKey("Price"))
                ModelState["Price"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    // A. ETKİNLİĞİ KAYDET
                    @event.SoldTicketCount = 0;
                    db.Events.Add(@event);
                    db.SaveChanges(); // ID oluştu

                    // B. FİYAT BÖLGELERİNİ KAYDET (SİNEMA HARİÇ)
                    if (@event.Category != "Sinema" && ZoneNames != null && ZonePrices != null)
                    {
                        for (int i = 0; i < ZoneNames.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(ZoneNames[i]))
                            {
                                var priceOption = new EventPrice
                                {
                                    EventId = @event.EventId,
                                    ZoneName = ZoneNames[i],
                                    Price = ZonePrices[i],
                                    Capacity = @event.Capacity
                                };
                                db.EventPrices.Add(priceOption);
                            }
                        }
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Hata durumunda (Örn: DB hatası) hatayı ekrana yazdır ki görelim
                    ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                }
            }

            // --- HATA VARSA DROPDOWNLARI TEKRAR YÜKLE ---
            // Kod tekrarını önlemek için helper metodunu çağırabiliriz veya kodlarını buraya kopyalayabilirsin.
            // Ben senin yukarıdaki kodlarını (Kategori, Şehir, Resim) buraya tekrar ekliyorum:

            // 1. Kategoriler
            List<SelectListItem> categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "Konser", Value = "Konser" },
                new SelectListItem { Text = "Sinema", Value = "Sinema" },
                new SelectListItem { Text = "Tiyatro", Value = "Tiyatro" },
                new SelectListItem { Text = "Spor", Value = "Spor" },
                new SelectListItem { Text = "Festival", Value = "Festival" }
            };
            ViewBag.CategoryList = categories;

            // 2. Şehirler
            string[] citiesArray = new string[] {
                "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Isparta", "Mersin", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce"
            };
            ViewBag.CityList = citiesArray.Select(c => new SelectListItem { Text = c, Value = c }).ToList();

            // 3. Resimler
            string folderPath = Server.MapPath("~/event_photos/");
            List<SelectListItem> imageFiles = new List<SelectListItem>();
            imageFiles.Add(new SelectListItem { Text = "- Bir Resim Seçiniz -", Value = "", Selected = string.IsNullOrEmpty(@event.ImageUrl) });

            if (System.IO.Directory.Exists(folderPath))
            {
                string[] allFiles = System.IO.Directory.GetFiles(folderPath);
                foreach (string file in allFiles)
                {
                    string extension = System.IO.Path.GetExtension(file).ToLower();
                    string fileName = System.IO.Path.GetFileName(file);
                    string fullPath = "/event_photos/" + fileName;

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".webp")
                    {
                        imageFiles.Add(new SelectListItem
                        {
                            Text = fileName,
                            Value = fullPath,
                            Selected = (@event.ImageUrl == fullPath)
                        });
                    }
                }
            }
            ViewBag.ImageFileList = imageFiles;

            return View(@event);
        }

        private void ListeleriDoldur(string seciliResim = null)
        {
            // 1. Kategoriler
            ViewBag.CategoryList = new List<SelectListItem> {
        new SelectListItem { Text = "Konser", Value = "Konser" },
        new SelectListItem { Text = "Sinema", Value = "Sinema" },
        // ... diğerleri
    };

            // 2. Şehirler
            string[] citiesArray = new string[] { "Adana", "Adıyaman", /* ... */ };
            ViewBag.CityList = citiesArray.Select(c => new SelectListItem { Text = c, Value = c }).ToList();

            // 3. Resimler
            string folderPath = Server.MapPath("~/event_photos/");
            List<SelectListItem> imageFiles = new List<SelectListItem>();
            imageFiles.Add(new SelectListItem { Text = "- Resim Seçiniz -", Value = "" });

            if (System.IO.Directory.Exists(folderPath))
            {
                var files = System.IO.Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    // Sadece resim uzantılarını al
                    string ext = System.IO.Path.GetExtension(file).ToLower();
                    if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                    {
                        string fileName = System.IO.Path.GetFileName(file);
                        string fullPath = "/event_photos/" + fileName;
                        imageFiles.Add(new SelectListItem
                        {
                            Text = fileName,
                            Value = fullPath,
                            Selected = (seciliResim == fullPath) // Seçili olanı işaretle
                        });
                    }
                }
            }
            ViewBag.ImageFileList = imageFiles;
        }
        // ------------------- DÜZENLEME (EDIT - GET) -------------------
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Etkinliği ve ona bağlı Fiyat Bölgelerini getiriyoruz
            Event @event = db.Events.Include("EventPrices").FirstOrDefault(e => e.EventId == id);

            if (@event == null) return HttpNotFound();

            // --- RESİM LİSTELEME (Mevcut kodun) ---
            string folderPath = Server.MapPath("~/event_photos/");
            List<SelectListItem> imageFiles = new List<SelectListItem>();
            imageFiles.Add(new SelectListItem { Text = "- Resim Seçiniz -", Value = "", Selected = string.IsNullOrEmpty(@event.ImageUrl) });

            if (System.IO.Directory.Exists(folderPath))
            {
                string[] files = System.IO.Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    string extension = System.IO.Path.GetExtension(file).ToLower();
                    string fileName = System.IO.Path.GetFileName(file);
                    string fullPath = "/event_photos/" + fileName;

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".webp")
                    {
                        imageFiles.Add(new SelectListItem
                        {
                            Text = fileName,
                            Value = fullPath,
                            Selected = (@event.ImageUrl == fullPath)
                        });
                    }
                }
            }
            ViewBag.ImageFileList = imageFiles;

            // --- KATEGORİ VE ŞEHİR LİSTELERİ (Create ile aynı olmalı ki dropdown dolsun) ---
            List<SelectListItem> categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "Konser", Value = "Konser" },
                new SelectListItem { Text = "Sinema", Value = "Sinema" },
                new SelectListItem { Text = "Tiyatro", Value = "Tiyatro" },
                new SelectListItem { Text = "Spor", Value = "Spor" },
                new SelectListItem { Text = "Festival", Value = "Festival" }
            };
            ViewBag.CategoryList = categories;

            string[] citiesArray = new string[] {
                "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Isparta", "Mersin", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce"
            };
            ViewBag.CityList = citiesArray.Select(c => new SelectListItem { Text = c, Value = c }).ToList();

            return View(@event);
        }

        // ------------------- DÜZENLEME (EDIT - POST) -------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind içinden Price'ı çıkardık, parametrelere SinglePrice ve Zone dizilerini ekledik
        public ActionResult Edit([Bind(Include = "EventId,Title,Artist,Description,Category,ImageUrl,Date,City,Venue,Capacity,DiscountRate,SoldTicketCount,SubCategory")] Event @event, decimal? SinglePrice, string[] ZoneNames, decimal[] ZonePrices)
        {
            // 1. FİYAT BELİRLEME
            if (@event.Category == "Sinema")
            {
                @event.Price = SinglePrice ?? 0;
            }
            else
            {
                if (ZonePrices != null && ZonePrices.Length > 0)
                {
                    @event.Price = ZonePrices.Min();
                }
                else
                {
                    @event.Price = 0;
                }
            }

            // ModelState Price hatasını temizle
            if (ModelState.ContainsKey("Price")) ModelState["Price"].Errors.Clear();

            if (ModelState.IsValid)
            {
                // A. ANA ETKİNLİĞİ GÜNCELLE
                db.Entry(@event).State = EntityState.Modified;

                // B. ESKİ FİYAT BÖLGELERİNİ SİL (En temiz güncelleme yöntemi: Sil ve Yeniden Ekle)
                var oldPrices = db.EventPrices.Where(x => x.EventId == @event.EventId).ToList();
                db.EventPrices.RemoveRange(oldPrices);

                // C. YENİ FİYAT BÖLGELERİNİ EKLE (Sinema Hariç)
                if (@event.Category != "Sinema" && ZoneNames != null && ZonePrices != null)
                {
                    for (int i = 0; i < ZoneNames.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(ZoneNames[i]))
                        {
                            var priceOption = new EventPrice
                            {
                                EventId = @event.EventId,
                                ZoneName = ZoneNames[i],
                                Price = ZonePrices[i],
                                Capacity = @event.Capacity
                            };
                            db.EventPrices.Add(priceOption);
                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Hata varsa listeleri tekrar doldur (Kod tekrarı olmasın diye yukarıdaki listeleri buraya da kopyalaman gerek, ya da metod yapabilirsin)
            // Edit(GET) içindeki list doldurma kodlarının aynısını buraya da eklemelisin.
            return RedirectToAction("Edit", new { id = @event.EventId }); // Kısa yol: Hata olursa sayfayı yenile
        }

        // ------------------- SİLME (DELETE) -------------------
        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Event @event = db.Events.Find(id);
            if (@event == null) return HttpNotFound();
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }


        // EventsController.cs dosyasının içine, diğer metodların altına ekle:

        public ActionResult Search(string categories, string startDate, string endDate)
        {
            // 1. Veritabanındaki tüm etkinlikleri çağır
            var events = db.Events.AsQueryable();

            // 2. TARİH FİLTRELEME (ARALIK MANTIĞI)
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start;
                if (DateTime.TryParse(startDate, out start))
                {
                    // Eğer bitiş tarihi seçilmediyse, sadece o günün etkinlikleri (Bitiş = Başlangıç)
                    // VEYA sadece o günden sonrasını getirmek istersen mantığı değiştirebilirsin.
                    // Biz burada: Bitiş yoksa sadece o günü, varsa aralığı baz alalım.

                    DateTime end = start; // Varsayılan: Bitiş, başlangıca eşittir (Tek gün seçimi)

                    if (!string.IsNullOrEmpty(endDate))
                    {
                        DateTime.TryParse(endDate, out end);
                    }

                    // Veritabanı sorgusu: Başlangıç'tan büyük, Bitiş'ten küçük olanlar
                    // Not: Bitiş tarihinin son saatini kapsamak için gün sonuna çekiyoruz (23:59:59)
                    end = end.Date.AddDays(1).AddTicks(-1);

                    events = events.Where(e => e.Date >= start && e.Date <= end);
                }
            }

            // 3. KATEGORİ FİLTRELEME (Aynı kalıyor)
            if (!string.IsNullOrEmpty(categories))
            {
                var decodedCategories = Server.UrlDecode(categories);
                var selectedCats = decodedCategories.Split(',');
                var eventList = events.ToList();

                var filteredEvents = eventList.Where(e =>
                    e.SubCategory != null &&
                    selectedCats.Any(cat => e.SubCategory.Contains(cat.Trim()))
                ).ToList();

                return View(filteredEvents);
            }

            return View(events.OrderBy(e => e.Date).ToList());
        }
        public ActionResult SeatSelection(int id)
        {
            // 1. Etkinliği bul
            var secilenEvent = db.Events.Find(id);
            if (secilenEvent == null) return HttpNotFound();

            var doluKoltuklar = db.Tickets
            .Where(t => t.EventId == id)
            .Select(t => t.SeatNumber)
            .ToList();

            // 3. Dolu koltuk listesini View'a taşı
            ViewBag.OccupiedSeats = doluKoltuklar;

            // 4. Sayfayı aç
            return View("SeatSelection1v", secilenEvent);
        }
        private List<CartItem> GetCartFromSession()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // 1. AYAKTA (Konser/Festival) İçin Sepete Ekle
        [HttpPost]
        public ActionResult AddToCartDirect(int eventId)
        {
            
            var eventItem = db.Events.Find(eventId);

            if (eventItem == null) return HttpNotFound();

           
            if (eventItem.SoldTicketCount >= eventItem.Capacity)
            {
                TempData["Error"] = "Biletler tükendi!";
                return RedirectToAction("Details", new { id = eventId });
            }

            var cart = GetCartFromSession();

            
            var newItem = new CartItem
            {
                Event = eventItem,
                SeatNumber = "Ayakta",
                Quantity = 1
            };

            cart.Add(newItem);
            Session["Cart"] = cart; // Sepeti güncelle

            TempData["Success"] = "Etkinlik sepete eklendi!";
            // İşlem bitince Detay sayfasına geri dön
            return RedirectToAction("Details", new { id = eventId });
        }

        // 2. KOLTUKLU (Sinema/Tiyatro) İçin Sepete Ekle
        [HttpPost]
        public ActionResult AddToCartSeated(int eventId, string selectedSeat)
        {
            var eventItem = db.Events.Find(eventId);
            if (eventItem == null) return HttpNotFound();

            var cart = GetCartFromSession();

            // Bu koltuk zaten sepette mi?
            bool alreadyInCart = cart.Any(x => x.Event.EventId == eventId && x.SeatNumber == selectedSeat);

            if (alreadyInCart)
            {
                TempData["Error"] = "Bu koltuk zaten sepetinizde!";
                // Hata varsa koltuk seçim sayfasına geri gönder
                return RedirectToAction("SeatSelection", new { id = eventId });
            }

            
            var newItem = new CartItem
            {
                Event = eventItem,
                SeatNumber = selectedSeat,
                Quantity = 1
            };

            cart.Add(newItem);
            Session["Cart"] = cart;

            TempData["Success"] = "Koltuk sepete eklendi!";
            // Başarılı olursa Sepet sayfasına git (Henüz CartController oluşturmadıysan hata verebilir)
            return RedirectToAction("Index", "Cart");
        }
    }
}