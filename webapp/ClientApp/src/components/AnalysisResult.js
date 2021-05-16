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
        return (
            <div class="container table-responsive py-5">
                <h2>Análise</h2>
                <table class="table table-bordered table-hover">
                    <thead class="thead-dark">
                        <tr>
                            <th scope="col">Regra</th>
                            <th scope="col">Descrição</th>
                            <th scope="col">Arquivo</th>
                            <th scope="col">Linha</th>
                        </tr>
                    </thead>
                    <tbody>
                        {analysisResult.map(r => renderTableRow(r))}
                    </tbody>
                </table>
            </div>
        )
    }

    const renderTableRow = (rowData) => {
        return (
            <tr>
                <th scope="row">{rowData.ruleName}</th>
                <td>{rowData.ruleDescription}</td>
                <td>{rowData.filePath}</td>
                <td>{rowData.lineNumber}</td>
            </tr>
        );
    }

    return (
        <div>
            {loading && <div>Analisando...</div>}
            {!loading && <div>{renderTable()}</div>}
        </div>
    );
}

export default AnalysisResult;