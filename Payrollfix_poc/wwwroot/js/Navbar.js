document.addEventListener('DOMContentLoaded', function () {
    var sidebar = document.getElementById('sidebar');
    var toggleBtn = document.getElementById('toggleBtn');

    // Toggle sidebar collapse
    toggleBtn.addEventListener('click', function () {
        sidebar.classList.toggle('collapsed');
    });
});
