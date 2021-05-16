import React, { useState, useEffect } from 'react';

const AnalysisResult = ({ slnPath, excludedProjects }) => {

    const [analysisResult, setAnalysisResult] = useState({});
    const [dataLoaded, setDataLoaded] = useState(false);
    const [loading, setLoading] = useState(true);

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

    return (
        <div>
            {loading && <div>Analisando...</div>}
            {!loading && <div>{JSON.stringify(analysisResult)}</div>}
        </div>
    );
}

export default AnalysisResult;