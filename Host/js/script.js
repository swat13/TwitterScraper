const url = 'http://localhost:8080/twitter/GetHotTopicsCount';
fetchData(false);
const timer = document.getElementById("timer");
setInterval(() => {
    fetchData(true);
    timer.innerHTML = "07";
}, 6000)
setInterval(() => {
    timer.innerHTML = "0" + (+timer.innerHTML - 1);
}, 1000)
const allCharts = [];
function fetchData(update) {
    fetch(url)
        .then((res) => res.json())
        .then(function (data) {
            for (const key in data) {
                if (update) {
                    const count = [];
                    const date = [];
                    for (const item of data[key].dailyCountList) {
                        count.push(item.count);
                        date.push(item.date);
                    }
                    allCharts[key].data.labels = date;
                    allCharts[key].data.datasets[0].data = count;
                    allCharts[key].update();
                } else
                    chart(`chart-${data[key].topicType}`, data[key].dailyCountList);
            }
        })
        .catch(err => console.log(err))
}
function chart(el, data) {
    const count = [];
    const date = [];
    for (const item of data) {
        count.push(item.count);
        date.push(item.date);
    }
    const ctx = document.getElementById(el).getContext('2d');
    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: date,
            datasets: [{
                label: 'Number of tweets',
                data: count,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
    allCharts.push(myChart)
}
console.log('%c Please hire me 😀! ',
    'background: #7C4DFF; color: #EDE7F6; font-size: 1rem;padding: .25rem');
