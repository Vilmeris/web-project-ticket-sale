
// 1. TEMA DEĞİŞTİRME 


document.addEventListener('DOMContentLoaded', () => {
    const themeToggleBtn = document.getElementById('theme-toggle');
    const body = document.body;
    const leftside = document.getElementById('leftid');
    const rightside = document.getElementById('rightid');
    const eventCard1 = document.getElementById('event-card1');
    const eventCard2 = document.getElementById('event-card2');
    const eventCard3 = document.getElementById('event-card3');
    const eventCard4 = document.getElementById('event-card4');
    const eventCard5 = document.getElementById('event-card5');
    const header = document.getElementById('üst-kısımid');
    const footer = document.getElementById('footerid');

   
    const themableElements = [
        body, leftside, rightside, eventCard1, eventCard2,
        eventCard3, eventCard4, eventCard5, header, footer
    ].filter(el => el !== null); 

    // Başlangıç teması
    const savedTheme = localStorage.getItem('theme') || 'light'; // Varsayılan 'light' 
    themableElements.forEach(el => el.setAttribute('data-theme', savedTheme));
    if (themeToggleBtn) {
        themeToggleBtn.textContent = savedTheme === 'dark' ? 'Aydınlık Moda Geç' : 'Karanlık Moda Geç';
    }


    // Buton bulunamazsa hata vermemesi için kontrol
    if (!themeToggleBtn) {
        console.error("ID'si 'theme-toggle' olan buton bulunamadı.");
        return; // Buton yoksa aşağıdaki kodu çalıştırma
    }

    themeToggleBtn.addEventListener('click', () => {
        // Mevcut temayı body'den kontrol et (veya herhangi bir elementten)
        const currentTheme = body.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

        // Listede var olan TÜM elementlerin temasını tek seferde değiştir
        themableElements.forEach(el => {
            el.setAttribute('data-theme', newTheme);
        });

        // Temayı kaydet ve buton metnini güncelle
        localStorage.setItem('theme', newTheme);
        themeToggleBtn.textContent = newTheme === 'dark' ? 'Aydınlık Moda Geç' : 'Karanlık Moda Geç';
    });
});

// 2. ARAMA ÇUBUĞU İŞLEVİ 


const searchBar = document.querySelector('.search-bar');

if (searchBar) {
    searchBar.addEventListener('keyup', (event) => {
        const searchTerm = event.target.value.trim();
        
        // Kullanıcı Enter'a bastığında arama işlemini tetikle
        if (event.key === 'Enter' && searchTerm.length > 0) {
           console.log(`Aranan Terim: "${searchTerm}"`);
           alert(`"${searchTerm}" için arama yapılıyor... (Bu, konsola yazdırıldı)`);

            
          
            
        } else if (searchTerm.length === 0) {
            // Arama kutusu boşaltılırsa loglama
            console.log("Arama kutusu boş.");
        }
    });
}


