import React, { useState, useEffect } from 'react';

const SeveretyLevel = {
    1: 'BLOCKER',
    2: 'CRITICAL',
    3: 'MAJOR',
    4: 'MINOR',
    5: 'INFO'
}

const Report = ({ analysisResult, setSelectedTab }) => {
    const [selectedFileGroup, setSelectedFileGroup] = useState(null);

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
                            <li class="nav-item">
                                <a class={`nav-link ${selectedFileGroup && selectedFileGroup.filePath === r.filePath && "active"}`}
                                    href="#"
                                    onClick={() => setSelectedFileGroup(r)}
                                >
                                    <span className="fileRuleCount">{r.ruleResults.length}</span>
                                    <span>{r.fileName}</span>
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
                {selectedFileGroup.ruleResults.sort((a, b) => a.severityLevel - b.severityLevel).map(r => (
                    <div class="listItem">
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
                                {r.ruleName}, {r.dpName}
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