import React, { useState } from 'react';
import AnalysisResult from './AnalysisResult';

export const Home = ({ triggerAnalysis }) => {
    const [slnPath, setSlnPath] = useState("C:\\Users\\erico\\source\\repos\\shadowsocks-windows\\shadowsocks-windows.sln");
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
                <div className="card shadow-sm" style={{ padding: 60, marginTop: 100, border: '3px solid black' }}>
                    <div style={{ width: 400 }}>
                        <h2><center>ORCA</center></h2>
                        <br />
                        <div className="form-group">
                            <label style={{fontWeight: 'bold'}} htmlFor="slnPathInput">Solution path (.sln)</label>
                            <input
                                id="slnPathInput"
                                className="form-control"
                                type="text"
                                value={slnPath}
                                onChange={(e) => setSlnPath(e.target.value)} style={{ width: '100%' }}
                            />
                        </div>
                        <div className="form-group">
                            <label style={{ fontWeight: 'bold' }} htmlFor="rulesFile">Rules file (.cs)</label>
                            <input
                                id="rulesFile"
                                className="form-control"
                                style={{ height: 45, width: '100%'}}
                                type="file"
                                onChange={(e) => null} 
                            />
                        </div>
                        <div className="form-group">
                            <label style={{ fontWeight: 'bold' }} htmlFor="excludedInput">Exclude projects:</label>
                            <textarea
                                id="excludedInput"
                                className="form-control"
                                placeholder="Separate projects with semicolon"
                                onChange={(e) => setExcludedProjects(e.target.value)}
                                style={{ width: '100%', height: '100px' }}
                            />
                        </div>
                        <div style={{display: 'flex', justifyContent: 'flex-end'}}>
                            <button
                                style={{ background: 'black' }}
                                className="btn btn-primary" onClick={() => setAnalysisTriggered(true)}>Analyze</button>
                        </div>
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