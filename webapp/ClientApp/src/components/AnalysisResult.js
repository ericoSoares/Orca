import React, { useState, useEffect } from 'react';

export const AnalysisResult = () => {

    const [analysisResult, setAnalysisResult] = useState({});
    const [dataLoaded, setDataLoaded] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            const response = await fetch('analysis');
            const data = await response.json();
            setAnalysisResult(data);
            setDataLoaded(true);
            setLoading(false);
        }
        if (!dataLoaded) {
            fetchData();
        }

    }, []);

    return (
        <div>
            {loading && <div>Analisando...</div>}
            {!loading && <div>{JSON.stringify(analysisResult)}</div>}
        </div>
    );
}