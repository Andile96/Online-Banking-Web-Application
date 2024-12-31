document.getElementById("notificationDropdown").addEventListener("shown.bs.dropdown", function () {
    fetch('@Url.Action("MarkNotificationsAsRead", "Notification")')
        .then(response => response.json())
        .then(data => {
           
        });
});
