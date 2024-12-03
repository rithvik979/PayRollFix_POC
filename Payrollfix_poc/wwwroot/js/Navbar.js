//document.addEventListener('DOMContentLoaded', function () {
//    var sidebar = document.getElementById('sidebar');
//    var toggleBtn = document.getElementById('toggleBtn');

//    // Toggle sidebar collapse
//    toggleBtn.addEventListener('click', function () {
//        sidebar.classList.toggle('collapsed');
//    });
//});

document.addEventListener('DOMContentLoaded', function () {
    var sidebar = document.getElementById('sidebar');
    var toggleBtn = document.getElementById('toggleBtn');

    // Check if elements exist before adding event listeners
    if (sidebar && toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            sidebar.classList.toggle('collapsed');
        });
    }
});
