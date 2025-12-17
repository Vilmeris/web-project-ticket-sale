const hamlet = document.getElementById("hamlet-container");
const hamletimg = document.getElementById("hamletimg");
const hamletinfo = document.getElementById("hamlet-info");

// Create the hover 
const hoverContent = document.createElement('div');
hoverContent.id = "hamlet-container1";
hoverContent.style.display = "flex";
hoverContent.style.color = "cyan";
hoverContent.style.fontSize = "20px";
hoverContent.innerHTML = "** İstanbul • 12 Mart • 20:30 **";
hoverContent.style.display = "none";

hamlet.appendChild(hoverContent);

hamlet.addEventListener("mouseenter", () => {
    hamletimg.style.display = "none"; // hides picture (no animation)
    hamletinfo.style.display = "none";
    hoverContent.style.display = "block"; // shows info
    hoverContent.style.animation = "animation2 4.5s ease-in-out forwards infinite"; // animates info
});

hamlet.addEventListener("mouseleave", () => {
    hamletimg.style.display = "block"; // shows picture
    hamletinfo.style.display = "block";
    hoverContent.style.display = "none"; // hides info
    hoverContent.style.animation = "none"; // stops animation
});


const baris = document.getElementById("barisma-container");
const barismaimg = document.getElementById("barismaimg");
const barismainfo = document.getElementById("barisma-info");

// Create the hover content once
const hoverContent1 = document.createElement('div');
hoverContent1.id = "barisma-container1";
hoverContent1.style.display = "flex";
hoverContent1.style.color = "cyan";
hoverContent1.style.fontSize = "20px";
hoverContent1.innerHTML = "** İstanbul • 12 Mart • 20:30 **";
hoverContent1.style.display = "none";

baris.appendChild(hoverContent1);

baris.addEventListener("mouseenter", () => {
    barismaimg.style.display = "none"; // hides picture (no animation)
    barismainfo.style.display = "none";
    hoverContent1.style.display = "block"; // shows info
    hoverContent1.style.animation = "animation2 4s ease-in-out forwards infinite"; // Animate info
});

baris.addEventListener("mouseleave", () => {
    barismaimg.style.display = "block"; // shows picture
    barismainfo.style.display = "block";
    hoverContent1.style.display = "none"; // hides info
    hoverContent1.style.animation = "none"; // stops animation
});


const galactic = document.getElementById("intergalactic-container");
const galacticimg = document.getElementById("galacticimg");
const galacticinfo = document.getElementById("intergalactic-info");

// Create the hover content once
const hoverContent2 = document.createElement('div');
hoverContent2.id = "intergalactic-container1";
hoverContent2.style.display = "flex";
hoverContent2.style.color = "cyan";
hoverContent2.style.fontSize = "20px";
hoverContent2.innerHTML = "** İstanbul • 12 Mart • 20:30 **";
hoverContent2.style.display = "none";

galactic.appendChild(hoverContent2);

galactic.addEventListener("mouseenter", () => {
    galacticimg.style.display = "none"; // hides picture (no animation)
    galacticinfo.style.display = "none";
    hoverContent2.style.display = "block"; // shows info
    hoverContent2.style.animation = "animation2 4s ease-in-out forwards infinite"; // Animate info
});

galactic.addEventListener("mouseleave", () => {
    galacticimg.style.display = "block"; // shows picture
    galacticinfo.style.display = "block";
    hoverContent2.style.display = "none"; // hides info
    hoverContent2.style.animation = "none"; // stops animation
});


const tempest = document.getElementById("tempest-container");
const tempestimg = document.getElementById("tempestimg");
const tempestinfo = document.getElementById("tempest-info");

// Create the hover content once
const hoverContent3 = document.createElement('div');
hoverContent3.id = "tempest-container1";
hoverContent3.style.display = "flex";
hoverContent3.style.color = "cyan";
hoverContent3.style.fontSize = "20px";
hoverContent3.innerHTML = "** İstanbul • 12 Mart • 20:30 **";
hoverContent3.style.display = "none";

tempest.appendChild(hoverContent3);

tempest.addEventListener("mouseenter", () => {
    tempestimg.style.display = "none"; // hides picture (no animation)
    tempestinfo.style.display = "none";
    hoverContent3.style.display = "block"; // shows info
    hoverContent3.style.animation = "animation2 4s ease-in-out forwards infinite"; // Animate info
});

tempest.addEventListener("mouseleave", () => {
    tempestimg.style.display = "block"; // shows picture
    tempestinfo.style.display = "block";
    hoverContent3.style.display = "none"; // hides info
    hoverContent3.style.animation = "none"; // stops animation
});

const starfall = document.getElementById("starfall-container");
const starfallimg = document.getElementById("starfallimg");
const starfallinfo = document.getElementById("starfall-info");

// Create the hover content once
const hoverContent4 = document.createElement('div');
hoverContent4.id = "starfall-container1";
hoverContent4.style.display = "flex";
hoverContent4.style.color = "cyan";
hoverContent4.style.fontSize = "20px";
hoverContent4.innerHTML = "** İstanbul • 12 Mart • 20:30 **";
hoverContent4.style.display = "none";

starfall.appendChild(hoverContent4);

starfall.addEventListener("mouseenter", () => {
    starfallimg.style.display = "none"; // hides picture (no animation)
    starfallinfo.style.display = "none";
    hoverContent4.style.display = "block"; // shows info
    hoverContent4.style.animation = "animation2 4s ease-in-out forwards infinite"; // Animate info
});

starfall.addEventListener("mouseleave", () => {
    starfallimg.style.display = "block"; // shows picture
    starfallinfo.style.display = "block";
    hoverContent4.style.display = "none"; // hides info
    hoverContent4.style.animation = "none"; // stops animation
});
