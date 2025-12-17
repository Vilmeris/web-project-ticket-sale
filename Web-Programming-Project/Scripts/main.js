// SAYFA YÜKLENDÜKTEN SONRA ÇALIŞMASI İÇİN
document.addEventListener('DOMContentLoaded', function() {

    
    document.querySelectorAll('.carousel-container').forEach(container => {
        
       
        const wrapper = container.querySelector('.carousel-wrapper');
        const prevBtn = container.querySelector('.carousel-btn-left');
        const nextBtn = container.querySelector('.carousel-btn-right');
        const slides = container.querySelectorAll('.event-slide');

        // Eğer bu carousel için gerekli elementler yoksa, hata ver ve bir sonrakine geç
        if (!wrapper || !prevBtn || !nextBtn || slides.length === 0) {
            console.error('Bir carousel için gerekli HTML elementleri bulunamadı:', container);
            return;
        }

        const totalSlides = slides.length;
        let currentIndex = 0; // Her carousel'in KENDİ currentIndex'i olacak

       
        const getVisibleCards = () => {
            if (window.innerWidth <= 768) return 1;
            if (window.innerWidth <= 1200) return 2;
            return 3;
        };

        function updateCarousel() {
            const cardWidth = slides[0].offsetWidth;
            const gap = 20; // HTML/CSS'teki gap ile aynı değer 
            const offset = currentIndex * (cardWidth + gap);
            wrapper.style.transform = `translateX(-${offset}px)`;
            
            // Butonları devre dışı bırak
            const visibleCards = getVisibleCards();
            const maxIndex = totalSlides - visibleCards;
            
            prevBtn.disabled = currentIndex === 0;
            nextBtn.disabled = currentIndex >= maxIndex;
            
            prevBtn.style.opacity = prevBtn.disabled ? '0.3' : '1';
            nextBtn.style.opacity = nextBtn.disabled ? '0.3' : '1';
            prevBtn.style.cursor = prevBtn.disabled ? 'not-allowed' : 'pointer';
            nextBtn.style.cursor = nextBtn.disabled ? 'not-allowed' : 'pointer';
        }

        function nextSlide() {
            const visibleCards = getVisibleCards();
            const maxIndex = totalSlides - visibleCards;
            if (currentIndex < maxIndex) {
                currentIndex++;
                updateCarousel();
            }
        }

        function prevSlide() {
            if (currentIndex > 0) {
                currentIndex--;
                updateCarousel();
            }
        }

        prevBtn.addEventListener('click', prevSlide);
        nextBtn.addEventListener('click', nextSlide);

        // Touch desteği
        let touchStartX = 0;
        let touchEndX = 0;

        wrapper.addEventListener('touchstart', (e) => {
            touchStartX = e.changedTouches[0].screenX;
        }, { passive: true });

        wrapper.addEventListener('touchend', (e) => {
            touchEndX = e.changedTouches[0].screenX;
            if (touchEndX < touchStartX - 50) { // Kaydırma mesafesi
                nextSlide();
            }
            if (touchEndX > touchStartX + 50) { 
                prevSlide();
            }
        });

      
        let resizeTimer;
        window.addEventListener('resize', () => {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(() => {
                
                currentIndex = 0; 
                updateCarousel();
            }, 250);
        });

        // İlk yükleme
        updateCarousel();
    });

document.querySelectorAll('.event-slide').forEach(slide => {
  const video = slide.querySelector('.event-trailer');
  if(video) {
    slide.addEventListener('mouseenter', () => {
      video.currentTime = 0;
      video.play();
      video.style.opacity = 1; // hover’da görünür
    });
    slide.addEventListener('mouseleave', () => {
      video.pause();
      video.currentTime = 0;
      video.style.opacity = 0; // çıkınca gizle
    });
  }
});

  

})

