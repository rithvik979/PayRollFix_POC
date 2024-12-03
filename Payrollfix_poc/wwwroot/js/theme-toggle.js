document.addEventListener("DOMContentLoaded", () => {
    const savedTheme = localStorage.getItem("theme");
    if (savedTheme) {
        document.body.classList.toggle("dark-theme", savedTheme === "dark");
    }
});

function toggleTheme() {
    const isDarkTheme = document.body.classList.toggle("dark-theme");
    localStorage.setItem("theme", isDarkTheme ? "dark" : "light");
}

document.getElementById("theme-toggle").addEventListener("click", toggleTheme);
