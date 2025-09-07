// site.js
window.addEventListener('load', () => {
    const preloader = document.getElementById('preloader');
    if (!preloader) return;

    setTimeout(() => preloader.classList.add('loaded'), 1500);
    setTimeout(() => preloader.remove(), 3000);
});
document.querySelectorAll('.floating-nav a').forEach(link => {
    link.addEventListener('click', () => {
        link.blur(); // remove focus after click
    });
});
