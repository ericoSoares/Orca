import React, { useState } from 'react';

export const Home = ({ triggerAnalysis }) => {
    const [slnPath, setSlnPath] = useState("C:\\Users\\erico\\source\\repos\\TestProject\\TestProject.sln");
    const [excludedProjects, setExcludedProjects] = useState("");
    return (
        <div
            style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                height: '100%',
                width: '100%'
            }}
        >
            <div>
                <div>Aponte o caminho da solution a ser analisada</div>
                <input
                    type="text"
                    value={slnPath}
                    onChange={(e) => setSlnPath(e.target.value)} style={{ width: '100%' }}
                />
                <div>Excluir projetos:</div>
                <textarea
                    onChange={(e) => setExcludedProjects(e.target.value)}
                    style={{ width: '100%', height: '100px' }}
                />
                <br />
                <button className="btn btn-primary" onClick={triggerAnalysis}>Analisar</button>
            </div>
        </div>
    );
}