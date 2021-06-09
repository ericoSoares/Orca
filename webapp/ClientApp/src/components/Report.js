import React, { useState, useEffect } from 'react';

const SeverityLevel = {
    1: 'Blocker',
    2: 'Critical',
    3: 'Major',
    4: 'Minor',
    5: 'Info'
}

const Report = ({ analysisResult, setSelectedTab }) => {
    const [selectedFileGroup, setSelectedFileGroup] = useState(null);
    const [severityFilter, setSeverityFilter] = useState(0);
    const [dpFilter, setDpFilter] = useState('');

    useEffect(() => {
        if (analysisResult.ruleResultGroups.length) {
            setSelectedFileGroup(analysisResult.ruleResultGroups[0])
        }
    }, []);

    const renderSeverityLevel = (level) => {
        switch (level) {
            case 1:
                return <span className="severityBlocker">BLOCKER</span>;
            case 2:
                return <span className="severityCritical">CRITICAL</span>;
            case 3:
                return <span className="severityMajor">MAJOR</span>;
            case 4:
                return <span className="severityMinor">MINOR</span>;
            case 5:
                return <span className="severityInfo">INFO</span>;
        }
    }

    const renderFilesMenu = () => {
        return (
            <nav class="col-md-2 d-none d-md-block bg-light sidebar">
                <div class="sidebar-sticky">
                    <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
                        <span>Files</span>
                        <a class="d-flex align-items-center text-muted" href="#">
                            <span data-feather="plus-circle"></span>
                        </a>
                    </h6>
                    <ul class="nav flex-column mb-2">
                        {analysisResult.ruleResultGroups.map(r => (
                            <li class="nav-item" key={Math.random()}>
                                <a class={`nav-link ${selectedFileGroup && selectedFileGroup.filePath === r.filePath && "active"}`}
                                    href="#"
                                    onClick={() => setSelectedFileGroup(r)}
                                >
                                    <span>{r.fileName}</span>
                                    <span className="fileRuleCount">{r.ruleResults.length}</span>
                                </a>
                            </li>    
                        ))}
                    </ul>
                </div>
            </nav>
        );
    }

    const renderList = () => {
        if (!selectedFileGroup) return <div></div>;
        return (
            <div>
                <div class="listHeader mb-4"><h4>File</h4><h3>{selectedFileGroup.filePath}</h3></div>
                <div className="listFilters">
                    <div class="dropdown">
                        <button class="btn btn-sm btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            {SeverityLevel[severityFilter] || 'Severity Level'}
                    </button>
                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                            <a class="dropdown-item" onClick={() => setSeverityFilter(0)}>All</a>
                            <a class="dropdown-item" onClick={() => setSeverityFilter(1)}>Blocker</a>
                            <a class="dropdown-item" onClick={() => setSeverityFilter(2)}>Critical</a>
                            <a class="dropdown-item" onClick={() => setSeverityFilter(3)}>Major</a>
                            <a class="dropdown-item" onClick={() => setSeverityFilter(4)}>Minor</a>
                            <a class="dropdown-item" onClick={() => setSeverityFilter(5)}>Info</a>
                        </div>
                    </div>
                    <div class="dropdown">
                        <button class="btn btn-sm btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            {dpFilter || 'Design Pattern'}
                    </button>
                        <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                            <a class="dropdown-item" onClick={() => setDpFilter('')}>All</a>  
                            {analysisResult.overview.designPatterns.map(r => (
                                <a key={Math.random()} class="dropdown-item" onClick={() => setDpFilter(r.name)}>{r.name}</a>  
                            ))}
                        </div>
                    </div>
                </div>
                {selectedFileGroup.ruleResults
                    .filter(r => r.dpName === dpFilter || dpFilter === '')
                    .filter(r => r.severityLevel === severityFilter || severityFilter === 0)
                    .sort((a, b) => a.severityLevel - b.severityLevel).map(r => (
                    <div class="listItem" key={Math.random()}>
                        <div class="itemHeader">
                            <p class="ruleDescription">
                                {r.ruleDescription}
                        </p>
                            <p class="ruleLineNumber">
                                Line {r.lineNumber}
                        </p>
                        </div>
                        <div class="itemBody">
                            <p class="secondLineInfo">
                                {r.ruleName}, <a href={r.dpExtraInfo} target="blank">{r.dpName} Pattern</a>
                        </p>
                            <p class="severityLevel">{renderSeverityLevel(r.severityLevel)}</p>
                        </div>
                    </div>    
                ))}
            </div>
            
        );
    }

    return (
        <div>
            {renderFilesMenu()}
            <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-4">
                {renderList()}
            </main>
        </div>
    );
}

export default Report;