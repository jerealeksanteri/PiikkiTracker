window.setupBalanceChart = (balanceData) => {
    const ctx = document.getElementById('balanceChart').getContext('2d');

    // Prepares the data for the chart
    const labels = balanceData.map(data => new Date(data.date).toLocaleDateString());
    const data = balanceData.map(data => data.balance);

    // Creates the chart using the Chart.js library
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Balance',
                data: data,
                borderColor: 'rgba(75, 192, 192, 1)',
                fill: false,
                tension: 0.1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
};

window.setupProductsColumnChart = (productData) => {
    const ctx = document.getElementById('userProductsChart').getContext('2d');

    const labels = productData.map(data => data.productName);
    const data = productData.map(data => data.totalSpent);

    const colorPalette = [
        'rgba(255, 99, 132, 0.6)',
        'rgba(54, 162, 235, 0.6)',
        'rgba(255, 206, 86, 0.6)',
        'rgba(75, 192, 192, 0.6)',
        'rgba(153, 102, 255, 0.6)',
        'rgba(255, 159, 64, 0.6)'
    ];

    const backgroundColors = productData.map((_, index) => colorPalette[index % colorPalette.length]);
    const borderColors = backgroundColors.map(color => color.replace('0.6', '1'));

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Total Spent (€)',
                data: data,
                backgroundColor: backgroundColors,
                borderColor: borderColors,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });
};