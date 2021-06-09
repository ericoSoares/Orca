import React, { useState } from 'react';
import { Pie } from 'react-chartjs-2';
import { Bar } from 'react-chartjs-2';



const getSeverityLevel = (level) => {
    switch (level) {
        case 1:
            return 'BLOCKER';
        case 2:
            return 'CRITICAL';
        case 3:
            return 'MAJOR';
        case 4:
            return 'MINOR';
        case 5:
            return 'INFO';
    }
}

const Overview = ({ overview }) => {
    const pieData = {
        labels: ['Inheritance', 'Implementation', 'Dependency', 'Instantiation', 'Reception'],
        datasets: [
            {
                label: '# of Votes',
                data: [overview.relationships.inheritances, overview.relationships.implementations, overview.relationships.dependencies, overview.relationships.instantiations, overview.relationships.receptions],
                backgroundColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192,1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)',
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

    const barsData = {
        labels: ['Blocker', 'Critical', 'Major', 'Minor', 'Info'],
        datasets: [
            {
                label: '# of detected opportunities',
                data: [overview.opportunitiesBlocker, overview.opportunitiesCritical, overview.opportunitiesMajor, overview.opportunitiesMinor, overview.opportunitiesInfo],
                backgroundColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                ],
                borderWidth: 1,
            },
        ],
    };
    return (
            <main role="main" class="col-10 offset-1 px-4 mb-4">
                <h4 className="mb-4">Dashboard</h4>
                <div className="container-fluid">
                    <div className="row">
                        <div className="card shadow-sm col-12 dashboardCard">
                        <h6>Opportunities</h6>
                        <Bar height="100" data={barsData} />
                        </div>
                    </div>

                    <div className="row">
                        <div className="card shadow-sm col-8 dashboardCard">
                            <h6>Entities</h6>
                            <div class="table-responsive" style={{height: 400, overflow: 'auto'}}>
                                <table class="table table-striped table-sm">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Project</th>
                                            <th>Type</th>
                                            <th>Name</th>
                                            <th>File</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {overview.entities.map((r, i) => (
                                            <tr>
                                                <td>{i + 1}</td>
                                                <td>{r.project}</td>
                                                <td>{r.type}</td>
                                                <td>{r.name}</td>
                                                <td>{r.file}</td>
                                            </tr>    
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div className="card shadow-sm col-4 dashboardCard">
                            <h6>Relationships</h6>
                            <Pie height="40" data={pieData} />
                        </div>
                    </div>

                    <div className="row">
                        <div className="card shadow-sm col-6 dashboardCard">
                            <h6>Rules</h6>
                            <div class="table-responsive" style={{ height: 400, overflow: 'auto' }}>
                                <table class="table table-striped table-sm">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Name</th>
                                            <th>Description</th>
                                            <th>Severity</th>
                                            <th>DP Name</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {overview.rules.map((r, i) => (
                                            <tr>
                                                <td>{i + 1}</td>
                                                <td>{r.name}</td>
                                                <td>{r.description}</td>
                                                <td>{getSeverityLevel(r.severityLevel)}</td>
                                                <td>{r.dpName}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div className="card shadow-sm col-6 dashboardCard">
                            <h6>Design Patterns</h6>
                            <div class="table-responsive" style={{ height: 400, overflow: 'auto' }}>
                                <table class="table table-striped table-sm">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Name</th>
                                            <th>Description</th>
                                            <th>More Info</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {overview.designPatterns.map((r, i) => (
                                            <tr>
                                                <td>{i+1}</td>
                                                <td>{r.name}</td>
                                                <td>{r.description}</td>
                                                <td><a href={r.moreInfoUrl} target="blank">{r.moreInfoUrl}</a></td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
    )
}

export default Overview;