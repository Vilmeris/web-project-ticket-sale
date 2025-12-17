document.addEventListener('DOMContentLoaded', function () {
    console.log("🚀 Script Başlatıldı - V3.0 Final");

    // ==================================================================
    // 1. TÜM ELEMENTLERİ EN BAŞTA TANIMLA (KARIŞIKLIĞI ÖNLEMEK İÇİN)
    // ==================================================================
    // Dropdown ve Inputlar
    const categoryDropdown = document.getElementById("Category");
    const hiddenInput = document.getElementById("SubCategoryInput");
    
    // Resim Elemanları
    const imageSelector = document.getElementById('imageSelector');
    const imgPreview = document.getElementById('imagePreview');
    const noImageText = document.getElementById('noImageText');
    const uploadZone = document.querySelector('.image-upload-zone');

    // Fiyat Alanları
    const singlePriceArea = document.getElementById("single-price-area");
    const multiPriceArea = document.getElementById("multi-price-area");
    const addPriceBtn = document.getElementById("add-price-row");
    const priceContainer = document.getElementById("price-rows-container");

    // Kategori Kartları
    const allCards = document.querySelectorAll(".kategori-card");
    const categoryHeaders = document.querySelectorAll(".kategori-card .kategori-baslik");
    const checkboxes = document.querySelectorAll(".alt-kategoriler input[type='checkbox']");

    // ==================================================================
    // 2. FONKSİYONLAR
    // ==================================================================

    // A. Resim Önizleme Fonksiyonu
    function updateImagePreview() {
        if (!imageSelector || !imgPreview || !noImageText) return;

        const selectedValue = imageSelector.value;
        // Eğer değer varsa ve boş değilse
        if (selectedValue && selectedValue !== "" && !selectedValue.startsWith("-")) {
            imgPreview.src = selectedValue;
            imgPreview.style.display = "block";
            noImageText.style.display = "none";
            if (uploadZone) {
                uploadZone.style.borderColor = "#f5a623"; // Turuncu
            }
        } else {
            imgPreview.style.display = "none";
            imgPreview.src = "";
            noImageText.style.display = "block";
            if (uploadZone) {
                uploadZone.style.borderColor = "#ddd"; // Gri
            }
        }
    }

    // B. Alt Kategori Kartlarını Aç/Kapa
    function updateCategoryCards() {
        if (!categoryDropdown) return;
        
        // Türkçe karakter sorununu önlemek için basit bir çeviri veya doğrudan value kullanımı
        const selectedCat = categoryDropdown.value.toLowerCase(); 

        // Tüm kartları gizle
        allCards.forEach(card => {
            card.style.display = "none";
            // Checkboxları temizle
            card.querySelectorAll("input[type='checkbox']").forEach(cb => cb.checked = false);
        });

        // Gizli inputu temizle
        if (hiddenInput) hiddenInput.value = "";

        // İlgili kartı göster
        if (selectedCat && selectedCat !== "") {
            // .sinema, .tiyatro gibi class'ı bul
            const targetCard = document.querySelector(`.kategori-card.${selectedCat}`);
            if (targetCard) {
                targetCard.style.display = "block";
                
                // Animasyonlu giriş için (Opsiyonel)
                setTimeout(() => { targetCard.classList.add('active'); }, 10);
            }
        }
    }

    // C. Fiyat Alanlarını Yönet (Sinema vs Diğerleri)
    function togglePriceInputs() {
        if (!categoryDropdown || !singlePriceArea || !multiPriceArea) return;

        const selectedVal = categoryDropdown.value; // Büyük/küçük harf önemli, Value neyse o

        if (selectedVal === "Sinema") {
            // Sinema -> Tek Fiyat Aç, Diğerini Kapat
            singlePriceArea.style.display = "block";
            multiPriceArea.style.display = "none";
        } else if (selectedVal === "" || selectedVal === "- Kategori Seçiniz -") {
            // Seçim Yok -> İkisini de Kapat
            singlePriceArea.style.display = "none";
            multiPriceArea.style.display = "none";
        } else {
            // Diğerleri (Tiyatro, Konser vb) -> Çoklu Fiyat Aç
            singlePriceArea.style.display = "none";
            multiPriceArea.style.display = "block";
        }
    }

    // ==================================================================
    // 3. OLAY DİNLEYİCİLERİ (EVENT LISTENERS)
    // ==================================================================

    // Resim Değişince
    if (imageSelector) {
        imageSelector.addEventListener('change', updateImagePreview);
    }

    // Kategori Değişince (HEM KARTLARI HEM FİYATLARI GÜNCELLE)
    if (categoryDropdown) {
        categoryDropdown.addEventListener("change", function() {
            console.log("Kategori değişti: " + this.value);
            updateCategoryCards();
            togglePriceInputs();
        });
    }

    // Accordion Başlık Tıklaması
    if (categoryHeaders.length > 0) {
        categoryHeaders.forEach(header => {
            header.addEventListener("click", function () {
                const card = this.closest(".kategori-card");
                if (card) {
                    // İçerik kısmını aç/kapa
                    const ul = card.querySelector('.alt-kategoriler');
                    if(ul) {
                        if(ul.style.display === "none" || ul.style.display === "") {
                            ul.style.display = "block";
                        } else {
                            ul.style.display = "none";
                        }
                    }
                }
            });
        });
    }

    // Checkbox Seçimleri
    if (checkboxes.length > 0) {
        checkboxes.forEach(cb => {
            cb.addEventListener("change", function () {
                let selectedValues = [];
                document.querySelectorAll(".alt-kategoriler input[type='checkbox']:checked").forEach(checkedBox => {
                    selectedValues.push(checkedBox.value);
                });
                if (hiddenInput) {
                    hiddenInput.value = selectedValues.join(", ");
                    console.log("Seçilen Alt Kategoriler: " + hiddenInput.value);
                }
            });
        });
    }

    // Yeni Bölge Ekleme Butonu
    if (addPriceBtn && priceContainer) {
        addPriceBtn.addEventListener("click", function () {
            const newRow = document.createElement("div");
            newRow.classList.add("row", "mb-2", "price-row");
            newRow.innerHTML = `
                <div class="col-md-5">
                    <input type="text" name="ZoneNames" class="form-control modern-input" placeholder="Bölge Adı (Örn: Balkon)" required>
                </div>
                <div class="col-md-4">
                    <div class="input-group">
                        <span class="input-group-text">₺</span>
                        <input type="number" name="ZonePrices" class="form-control modern-input" placeholder="Fiyat" required min="0">
                    </div>
                </div>
                <div class="col-md-2 d-flex align-items-center">
                    <button type="button" class="btn btn-sm btn-danger remove-row" style="border-radius: 50%; width: 30px; height: 30px; font-weight:bold; display:flex; align-items:center; justify-content:center;">×</button>
                </div>
            `;
            priceContainer.appendChild(newRow);

            // Yeni eklenen butonun silme özelliği
            newRow.querySelector(".remove-row").addEventListener("click", function () {
                newRow.remove();
            });
        });
    }

    // ==================================================================
    // 4. BAŞLANGIÇ KONTROLLERİ (SAYFA YÜKLENİNCE ÇALIŞACAKLAR)
    // ==================================================================
    setTimeout(() => {
        updateImagePreview();
        updateCategoryCards();
        togglePriceInputs();
    }, 100);

});