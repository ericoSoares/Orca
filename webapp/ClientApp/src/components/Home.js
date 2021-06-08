import React, { useState } from 'react';
import AnalysisResult from './AnalysisResult';

export const Home = ({ triggerAnalysis }) => {
    const [slnPath, setSlnPath] = useState("C:\\Users\\erico\\source\\repos\\clean-architecture-manga\\Clean-Architecture-Manga.sln");
    const [excludedProjects, setExcludedProjects] = useState("ComponentTests;IntegrationTests;UnitTests");
    const [analysisTriggered, setAnalysisTriggered] = useState(false);

    const renderForm = () => {
        return (
            <div
                style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    height: '100%',
                    width: '100%',
                }}
            >
                <div className="card shadow-sm" style={{ padding: 60 }}>
                    <div style={{ width: 400 }}>
                        <div className="form-group">
                            <label htmlFor="slnPathInput">Caminho da Solution (.sln)</label>
                            <input
                                id="slnPathInput"
                                className="form-control"
                                type="text"
                                value={slnPath}
                                onChange={(e) => setSlnPath(e.target.value)} style={{ width: '100%' }}
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="excludedInput">Excluir projetos:</label>
                            <textarea
                                id="excludedInput"
                                className="form-control"
                                placeholder="separar projetos com ;"
                                onChange={(e) => setExcludedProjects(e.target.value)}
                                style={{ width: '100%', height: '100px' }}
                            />
                        </div>
                        <button className="btn btn-primary" onClick={() => setAnalysisTriggered(true)}>Analisar</button>
                    </div>
                </div>
            </div>
        );
    }

    if (analysisTriggered) {
        return (<AnalysisResult slnPath={slnPath} setAnalysisTriggered={setAnalysisTriggered} excludedProjects={excludedProjects} />);
    } else {
        return renderForm();
    }
}