import React, { useState, useEffect } from 'react';
import Report from './Report';
import Overview from './Overview';

const AnalysisResult = ({ slnPath, excludedProjects, setAnalysisTriggered }) => {

    const [analysisResult, setAnalysisResult] = useState({});
    const [dataLoaded, setDataLoaded] = useState(false);
    const [loading, setLoading] = useState(true);
    const [selectedTab, setSelectedTab] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            const response = await fetch('analysis?slnPath=' + slnPath + '&excluded=' + excludedProjects);
            const data = await response.json();
            setAnalysisResult(data);
            setLoading(false);
        }
        if (!dataLoaded) {
            fetchData();
            setDataLoaded(true);
        }

    }, [dataLoaded, excludedProjects, slnPath]);

    if (loading) return <div className="loadingScreen">Analyzing...</div>;

    return (
        <div style={{paddingBottom: 20}}>
            <nav className="navbar navbar-dark fixed-top bg-dark flex-md-nowrap p-0 shadow">
                <a className="navbar-brand col-sm-3 col-md-2 mr-0" href="#">ORCA</a>
                <div className="navbarListContainer">
                    <ul className="navbar-nav px-3">
                        <li className="nav-item px-2 text-nowrap">
                            <a className="nav-link" href="#" onClick={() => setSelectedTab(0)}>Overview</a>
                        </li>
                        <li className="nav-item px-3 text-nowrap">
                            <a className="nav-link" href="#" onClick={() => setSelectedTab(1)}>Report</a>
                        </li>
                    </ul>
                    <ul className="navbar-nav px-3">
                        <li className="nav-item text-nowrap">
                            <a className="nav-link" href="#" onClick={() => setAnalysisTriggered(false)}>New Analysis</a>
                        </li>
                    </ul>
                </div>
            </nav>

            {selectedTab === 0 && <Overview overview={analysisResult.overview} />}
            {selectedTab === 1 && <Report analysisResult={analysisResult} />}
        </div>
    );
}

export default AnalysisResult;