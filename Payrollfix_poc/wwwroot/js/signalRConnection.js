const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.start()
    .then(function () {
        console.log("SignalR kdsjfhlsd Connected.");
    })
    .catch(function (err) {
        console.error("Error connecting to SignalR:", err.toString());
    });

// Listen for leave status updates
connection.on("ReceiveLeaveStatusUpdate", (leaveId, newStatus) => {
    console.log(`Leave ID: ${leaveId} status updated to: ${newStatus}`);
    // Update the UI based on the new status
    document.querySelector(`#leaveStatus-${leaveId}`).innerText = newStatus;
});

// Listen for expense status updates
connection.on("ReceiveExpenseStatusUpdate", (expenseId, newStatus) => {
    console.log(`Expense ID: ${expenseId} status updated to: ${newStatus}`);
    // Update the UI based on the new status
    document.querySelector(`#expenseStatus-${expenseId}`).innerText = newStatus;
});
