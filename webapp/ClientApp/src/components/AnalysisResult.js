import React, { useState, useEffect } from 'react';
import Report from './Report';
import Overview from './Overview';

const AnalysisResult = ({ slnPath, excludedProjects, setAnalysisTriggered }) => {

    const [analysisResult, setAnalysisResult] = useState({});
    const [dataLoaded, setDataLoaded] = useState(false);
    const [loading, setLoading] = useState(true);
    const [selectedTab, setSelectedTab] = useState(1);

    useEffect(() => {
        const fetchData = async () => {
            console.log(slnPath, excludedProjects);
            const response = await fetch('analysis?slnPath=' + slnPath + '&excluded=' + excludedProjects);
            const data = await response.json();
            setAnalysisResult(data);
            setLoading(false);
        }
        if (!dataLoaded) {
            fetchData();
            setDataLoaded(true);
        }

    }, []);

    if (loading) return <div className="loadingScreen">Analisando...</div>;

    return (
        <div>
            <nav class="navbar navbar-dark fixed-top bg-dark flex-md-nowrap p-0 shadow">
                <a class="navbar-brand col-sm-3 col-md-2 mr-0" href="#">RePattern</a>
                <div className="navbarListContainer">
                    <ul class="navbar-nav px-3">
                        <li class="nav-item px-2 text-nowrap">
                            <a class="nav-link" href="#" onClick={() => setSelectedTab(0)}>Overview</a>
                        </li>
                        <li class="nav-item px-3 text-nowrap">
                            <a class="nav-link" href="#" onClick={() => setSelectedTab(1)}>Report</a>
                        </li>
                    </ul>
                    <ul class="navbar-nav px-3">
                        <li class="nav-item text-nowrap">
                            <a class="nav-link" href="#" onClick={() => setAnalysisTriggered(false)}>New Analysis</a>
                        </li>
                    </ul>
                </div>
            </nav>

            {selectedTab === 0 && <Overview analysisResult={analysisResult} />}
            {selectedTab === 1 && <Report analysisResult={analysisResult} />}
        </div>
    );
}

export default AnalysisResult;