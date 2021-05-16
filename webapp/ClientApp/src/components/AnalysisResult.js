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

    const renderTable = () => {
        console.log(analysisResult);
        return analysisResult.map(r => {
            return (
                <div>{`${r.ruleName} - ${r.filePath} - ${r.lineNumber}`}</div>
            )
        })
    }

    return (
        <div>
            {loading && <div>Analisando...</div>}
            {!loading && <div>{renderTable()}</div>}
        </div>
    );
}

export default AnalysisResult;