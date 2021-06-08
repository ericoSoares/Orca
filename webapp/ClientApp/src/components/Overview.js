import React, { useState } from 'react';
import { Pie } from 'react-chartjs-2';
import { Bar } from 'react-chartjs-2';

const data = {
    labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
    datasets: [
        {
            label: '# of Votes',
            data: [12, 19, 3, 5, 2, 3],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)',
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)',
            ],
            borderWidth: 1,
        },
    ],
};

const options = {
    scales: {
        yAxes: [
            {
                ticks: {
                    beginAtZero: true,
                },
            },
        ],
    },
};

const data2 = {
    labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
    datasets: [
        {
            label: '# of Votes',
            data: [12, 19, 3, 5, 2, 3],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)',
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)',
            ],
            borderWidth: 1,
        },
    ],
};

const Overview = ({ overview }) => {
    return (
        <div>
            <main role="main" class="col-10 offset-1 px-4">
                <h4 className="mb-4">Dashboard</h4>
                <div className="container">
                    <div className="row gx-5">
                        <div className="card shadow-sm col-12 dashboardCard">
                            <h6>Opportunities</h6>
                        </div>
                    </div>

                    <div className="row gx-5">
                        <div className="card shadow-sm col-4 dashboardCard">
                            <h6>AnalysedFiles</h6>
                        </div>
                        <div className="card shadow-sm col-4 dashboardCard">
                            <h6>Entities</h6>
                        </div>
                        <div className="card shadow-sm col-4 dashboardCard">
                            <h6>Relationships</h6>
                        </div>
                    </div>

                    <div className="row gx-5">
                        <div className="card shadow-sm col-6 dashboardCard">
                            <h6>Rules</h6>
                        </div>
                        <div className="card shadow-sm col-6 dashboardCard">
                            <h6>Design Patterns</h6>
                        </div>
                    </div>
                </div>
            </main>
        </div>
    )
}

export default Overview;